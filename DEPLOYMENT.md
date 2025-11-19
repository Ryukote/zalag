# Zalagaonica - Docker Deployment Guide

This guide will help you deploy the Zalagaonica application on a Linux server using Docker.

## Prerequisites

- Linux server (Ubuntu 20.04+ recommended)
- Docker Engine 20.10+
- Docker Compose 2.0+
- At least 2GB RAM
- At least 10GB free disk space

## Quick Start

### 1. Install Docker and Docker Compose (if not already installed)

```bash
# Update package index
sudo apt-get update

# Install Docker
curl -fsSL https://get.docker.com -o get-docker.sh
sudo sh get-docker.sh

# Install Docker Compose
sudo curl -L "https://github.com/docker/compose/releases/latest/download/docker-compose-$(uname -s)-$(uname -m)" -o /usr/local/bin/docker-compose
sudo chmod +x /usr/local/bin/docker-compose

# Add your user to docker group (optional, to run docker without sudo)
sudo usermod -aG docker $USER
# Log out and log back in for this to take effect
```

### 2. Copy Files to Server

Copy the entire `zalagaonica` directory to your Linux server:

```bash
# From your local machine
scp -r zalagaonica/ user@your-server-ip:/home/user/

# Or use rsync for better performance
rsync -avz --progress zalagaonica/ user@your-server-ip:/home/user/zalagaonica/
```

### 3. Configure Environment Variables

```bash
cd /home/user/zalagaonica

# Copy the example environment file
cp .env.example .env

# Edit the .env file with your settings
nano .env
```

**Important:** Change these values in `.env`:
- `DB_PASSWORD`: Set a strong database password
- `JWT_SECRET`: Set a strong, random secret key (at least 32 characters)
- `REACT_APP_API_URL`: Set to your server's IP or domain (e.g., http://your-server-ip:5000/api)

Example `.env` file:
```env
DB_PASSWORD=YourSecurePassword123!
JWT_SECRET=YourVeryLongAndRandomJWTSecretKey123456789!
JWT_ISSUER=zalagaonica.hr
JWT_AUDIENCE=zalagaonica-app
FISKALIZACIJA_BASE_URI=https://api.fiskalizacija.hr
FISKALIZACIJA_AUTH_TOKEN=
REACT_APP_API_URL=http://your-server-ip:5000/api
```

### 4. Deploy the Application

```bash
# Start all services
docker-compose up -d

# View logs
docker-compose logs -f

# Check service status
docker-compose ps
```

### 5. Access the Application

- **Frontend**: http://your-server-ip:3000
- **Backend API**: http://your-server-ip:5000
- **Swagger Documentation**: http://your-server-ip:5000/swagger

**Default Admin Credentials:**
- Email: `admin@pawnshop.hr`
- Password: `Admin123!`

**⚠️ Change the admin password immediately after first login!**

## Service Architecture

The application consists of three services:

1. **database**: PostgreSQL 16 database
   - Port: 5432
   - Volume: `zalagaonica-postgres-data`
   - Health check: Ensures database is ready before starting backend

2. **backend**: .NET 8 API
   - Port: 5000 (mapped to internal 80)
   - Depends on: database (healthy)
   - Features: Auto-migration, admin user seeding, JWT authentication
   - Health endpoints: `/api/health`, `/api/health/ready`, `/api/health/live`

3. **frontend**: React application (served via nginx)
   - Port: 3000 (mapped to internal 80)
   - Depends on: backend (healthy)
   - Features: React Router support, gzip compression

## Service Startup Order

The `docker-compose.yml` ensures proper startup order:

1. **Database** starts first and must be healthy (responding to `pg_isready`)
2. **Backend** starts only after database is healthy
3. **Frontend** starts only after backend is healthy

This prevents connection errors and ensures smooth deployment.

## Docker Commands

### Start Services
```bash
# Start all services in detached mode
docker-compose up -d

# Start specific service
docker-compose up -d backend

# Rebuild and start (after code changes)
docker-compose up -d --build
```

### Stop Services
```bash
# Stop all services
docker-compose down

# Stop and remove volumes (⚠️ deletes all data!)
docker-compose down -v
```

### View Logs
```bash
# All services
docker-compose logs -f

# Specific service
docker-compose logs -f backend
docker-compose logs -f frontend
docker-compose logs -f database

# Last 100 lines
docker-compose logs --tail=100 backend
```

### Service Management
```bash
# Restart service
docker-compose restart backend

# Check status
docker-compose ps

# Execute command in running container
docker-compose exec backend bash
docker-compose exec database psql -U postgres -d ZalagaonicaDB
```

## Database Management

### Backup Database
```bash
# Create backup
docker-compose exec -T database pg_dump -U postgres ZalagaonicaDB > backup_$(date +%Y%m%d_%H%M%S).sql

# Or with compression
docker-compose exec -T database pg_dump -U postgres ZalagaonicaDB | gzip > backup_$(date +%Y%m%d_%H%M%S).sql.gz
```

### Restore Database
```bash
# Stop backend first
docker-compose stop backend

# Restore from backup
cat backup.sql | docker-compose exec -T database psql -U postgres ZalagaonicaDB

# Or from compressed backup
gunzip < backup.sql.gz | docker-compose exec -T database psql -U postgres ZalagaonicaDB

# Start backend
docker-compose start backend
```

### Connect to Database
```bash
# Using psql
docker-compose exec database psql -U postgres -d ZalagaonicaDB

# Using external client (e.g., DBeaver, pgAdmin)
Host: your-server-ip
Port: 5432
Database: ZalagaonicaDB
Username: postgres
Password: (from .env file)
```

## Updating the Application

### Update Code
```bash
# Pull latest code
cd /home/user/zalagaonica
git pull origin main  # if using git

# Rebuild and restart
docker-compose down
docker-compose up -d --build

# Or rebuild specific service
docker-compose up -d --build backend
```

### Database Migrations
The backend automatically runs migrations on startup. If you need to manually run migrations:

```bash
# Enter backend container
docker-compose exec backend bash

# Run migrations
dotnet ef database update

# Exit container
exit
```

## Monitoring

### Health Checks
```bash
# Backend health
curl http://localhost:5000/api/health

# Backend readiness
curl http://localhost:5000/api/health/ready

# Backend liveness
curl http://localhost:5000/api/health/live

# Frontend health
curl http://localhost:3000/

# Database health
docker-compose exec database pg_isready -U postgres
```

### Resource Usage
```bash
# View resource usage
docker stats

# View disk usage
docker system df

# Clean up unused resources
docker system prune -a
```

## Firewall Configuration

If using UFW (Ubuntu Firewall):

```bash
# Allow SSH (if not already allowed)
sudo ufw allow 22/tcp

# Allow HTTP traffic
sudo ufw allow 3000/tcp  # Frontend
sudo ufw allow 5000/tcp  # Backend API

# Enable firewall
sudo ufw enable

# Check status
sudo ufw status
```

## Production Deployment Recommendations

### 1. Use HTTPS
Set up a reverse proxy (nginx or Traefik) with Let's Encrypt SSL certificates:

```bash
# Install nginx
sudo apt-get install nginx certbot python3-certbot-nginx

# Get SSL certificate
sudo certbot --nginx -d your-domain.com
```

### 2. Change Default Ports
Update `docker-compose.yml` to use non-standard ports or use a reverse proxy.

### 3. Secure Database
- Don't expose database port (5432) to the internet
- Use strong passwords
- Regular backups

### 4. Set Up Monitoring
Consider setting up:
- Prometheus + Grafana for metrics
- ELK Stack for log aggregation
- Uptime monitoring (e.g., UptimeRobot)

### 5. Automated Backups
Create a cron job for regular database backups:

```bash
# Edit crontab
crontab -e

# Add daily backup at 2 AM
0 2 * * * cd /home/user/zalagaonica && docker-compose exec -T database pg_dump -U postgres ZalagaonicaDB | gzip > /home/user/backups/zalagaonica_$(date +\%Y\%m\%d).sql.gz
```

### 6. Use Docker Secrets
For production, consider using Docker secrets instead of environment variables for sensitive data.

## Troubleshooting

### Services Won't Start
```bash
# Check logs
docker-compose logs

# Check individual service
docker-compose logs backend

# Restart services
docker-compose restart
```

### Database Connection Errors
```bash
# Ensure database is healthy
docker-compose ps

# Check database logs
docker-compose logs database

# Restart database
docker-compose restart database
```

### Frontend Can't Connect to Backend
- Check `REACT_APP_API_URL` in `.env`
- Ensure it points to your server's IP or domain
- Rebuild frontend: `docker-compose up -d --build frontend`

### Permission Issues
```bash
# Fix file permissions
sudo chown -R $USER:$USER /home/user/zalagaonica

# Fix Docker socket permissions
sudo chmod 666 /var/run/docker.sock
```

### Out of Disk Space
```bash
# Remove unused containers, images, and volumes
docker system prune -a

# Remove specific volumes
docker volume ls
docker volume rm volume_name
```

## Support

For issues and questions:
- Check logs: `docker-compose logs`
- Review this documentation
- Check Docker and PostgreSQL documentation

## License

[Your License Here]
