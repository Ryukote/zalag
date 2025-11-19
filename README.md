# Zalagaonica - Pawn Shop Management System

A comprehensive pawn shop (zalagaonica) management system built with .NET 8 and React, designed for Croatian market with built-in support for Croatian bookkeeping practices and regulations.

## Features

### Core Functionality
- 📦 **Inventory Management** - Complete article/item tracking with warehouse management
- 👥 **Client Management** - Customer and partner relationship management
- 💰 **Pledge Operations** - Pawn transactions with redemption and forfeiture workflows
- 📄 **Document Management** - Incoming and outgoing document processing
- 💵 **Financial Tracking** - Sales, purchases, loans, repayments, and expenses
- 🧾 **Cash Register** - Complete cash transaction management
- 👨‍💼 **HR Management** - Employee management, payroll, and vacation tracking
- 📊 **Reporting** - PDF report generation for various documents
- 🔐 **Authentication** - Secure JWT-based authentication with role management

### Technical Features
- ✅ Multi-container Docker deployment
- ✅ Automatic database migrations
- ✅ Health checks and service orchestration
- ✅ RESTful API with Swagger documentation
- ✅ Responsive React frontend with modern UI
- ✅ PostgreSQL database with proper relationships
- ✅ Croatian language support

## Technology Stack

### Backend
- .NET 8 (C#)
- Entity Framework Core
- PostgreSQL 16
- JWT Authentication
- Swagger/OpenAPI

### Frontend
- React 18
- TypeScript
- Tailwind CSS
- React Router
- Heroicons

### Infrastructure
- Docker & Docker Compose
- Nginx (for frontend)
- Linux (Ubuntu recommended)

## Quick Start

### Prerequisites
- Docker 20.10+
- Docker Compose 2.0+
- 2GB RAM minimum
- 10GB disk space

### Installation

1. **Clone or copy the repository to your server**
```bash
cd /home/user
# Upload the zalagaonica directory here
```

2. **Configure environment**
```bash
cd zalagaonica
cp .env.example .env
nano .env  # Edit with your settings
```

3. **Start the application**
```bash
chmod +x *.sh
./start.sh
```

4. **Access the application**
- Application: http://localhost:3333
- API: http://localhost:3333/api
- Swagger Docs: http://localhost:3333/swagger

**Note**: Everything is served through nginx on port 3333

### Default Credentials
- **Email**: admin@pawnshop.hr
- **Password**: Admin123!

⚠️ **Change the admin password immediately after first login!**

## Management Scripts

The application includes several convenient scripts for management:

### start.sh
Start all services with proper health checks and dependency management.
```bash
./start.sh
```

### stop.sh
Stop all services gracefully.
```bash
./stop.sh
```

### logs.sh
View application logs in real-time.
```bash
# All services
./logs.sh

# Specific service
./logs.sh backend
./logs.sh frontend
./logs.sh database
```

### backup.sh
Create a database backup with automatic rotation (keeps last 7 backups).
```bash
./backup.sh
```

### restore.sh
Restore database from a backup file.
```bash
./restore.sh backups/zalagaonica_backup_20250117_120000.sql.gz
```

## Service Architecture

The application consists of four Docker services:

### 1. Database (PostgreSQL 16)
- **Container**: zalagaonica-db
- **Port**: 5432 (internal only)
- **Volume**: zalagaonica-postgres-data
- **Health Check**: pg_isready

### 2. Backend (.NET 8 API)
- **Container**: zalagaonica-backend
- **Port**: 80 (internal only, accessed via nginx)
- **Depends on**: database (healthy)
- **Features**:
  - Auto-migration on startup
  - Admin user seeding
  - JWT authentication
  - Swagger documentation
  - Health endpoints

### 3. Frontend (React + Nginx)
- **Container**: zalagaonica-frontend
- **Port**: 80 (internal only, accessed via nginx)
- **Depends on**: backend (healthy)
- **Features**:
  - React Router support
  - Optimized build
  - SPA routing support

### 4. Nginx Reverse Proxy
- **Container**: zalagaonica-nginx
- **Port**: 3333 → 80 (only exposed port)
- **Depends on**: frontend + backend (both healthy)
- **Routes**:
  - `/` → Frontend
  - `/api/*` → Backend API
  - `/swagger` → Backend Swagger
  - `/health` → Nginx health
- **Features**:
  - Single entry point
  - No CORS issues
  - Gzip compression
  - Security headers
  - Static asset caching (1 year)
  - Request buffering

## Startup Order & Health Checks

The system ensures proper startup order:

1. **Database** starts and waits until healthy (pg_isready check)
2. **Backend** starts only after database is healthy
   - Runs database migrations
   - Seeds admin user and role
   - Health endpoint: `/api/health`
3. **Frontend** starts only after backend is healthy
   - Serves optimized React build
   - Handles React Router
4. **Nginx** starts only after frontend and backend are healthy
   - Routes all traffic
   - Single entry point on port 3333

This prevents connection errors and ensures smooth operation.

### Port Architecture

```
External: Port 3333 (nginx)
    ↓
Nginx Reverse Proxy
    ↓
    ├─→ Frontend (port 80 internal)
    └─→ Backend (port 80 internal)
            ↓
        Database (port 5432 internal)
```

See [NGINX_ARCHITECTURE.md](NGINX_ARCHITECTURE.md) for detailed information.

## API Documentation

Once running, access the Swagger documentation at:
```
http://localhost:3333/swagger
```

### Health Endpoints (all through port 3333)
- `/health` - Nginx health check
- `/api/health` - Overall backend health status
- `/api/health/ready` - Readiness check (includes migrations)
- `/api/health/live` - Liveness check

## Database Management

### Manual Backup
```bash
docker-compose exec -T database pg_dump -U postgres ZalagaonicaDB > backup.sql
```

### Manual Restore
```bash
cat backup.sql | docker-compose exec -T database psql -U postgres ZalagaonicaDB
```

### Access Database
```bash
docker-compose exec database psql -U postgres -d ZalagaonicaDB
```

### View Migrations
```bash
docker-compose exec backend dotnet ef migrations list
```

## Configuration

### Environment Variables

Create a `.env` file based on `.env.example`:

```env
# Database
DB_PASSWORD=your_secure_password

# JWT Authentication
JWT_SECRET=your_very_long_random_secret_key
JWT_ISSUER=zalagaonica.hr
JWT_AUDIENCE=zalagaonica-app

# Nginx Port
NGINX_PORT=3333

# Frontend API URL (use /api for relative path through nginx)
REACT_APP_API_URL=/api

# Fiskalizacija (Croatian Fiscalization)
FISKALIZACIJA_BASE_URI=https://api.fiskalizacija.hr
FISKALIZACIJA_AUTH_TOKEN=your_token_here
```

### Port Configuration

The application exposes only **one port (3333)** through nginx:
- **3333**: Application entry point (nginx reverse proxy)
  - Routes to frontend, backend, and API docs

Internal ports (not exposed to host):
- Frontend: 80 (internal)
- Backend: 80 (internal)
- Database: 5432 (optionally exposed for admin)

## Production Deployment

For production use:

1. **Use production compose file**
```bash
docker-compose -f docker-compose.yml -f docker-compose.prod.yml up -d
```

2. **Set up HTTPS with reverse proxy** (nginx/Traefik + Let's Encrypt)

3. **Configure firewall**
```bash
sudo ufw allow 22/tcp   # SSH
sudo ufw allow 80/tcp   # HTTP
sudo ufw allow 443/tcp  # HTTPS
sudo ufw enable
```

4. **Set up automated backups** (add to crontab)
```bash
0 2 * * * cd /home/user/zalagaonica && ./backup.sh
```

5. **Monitor services**
- Set up Prometheus + Grafana
- Configure log aggregation
- Set up uptime monitoring

## Troubleshooting

### Services won't start
```bash
docker-compose logs
docker-compose ps
```

### Database connection issues
```bash
docker-compose restart database
docker-compose logs database
```

### Frontend can't connect to backend
1. Check `REACT_APP_API_URL` in `.env` (should be `/api`)
2. Ensure nginx is running: `docker-compose ps nginx`
3. Check nginx logs: `docker-compose logs nginx`
4. Rebuild if needed: `docker-compose up -d --build`

### Reset everything
```bash
docker-compose down -v
docker-compose up -d --build
```

## File Structure

```
zalagaonica/
├── backend/
│   ├── Dockerfile
│   ├── .dockerignore
│   └── Zalagaonica.Backend/
│       ├── Zalagaonica.Backend/     # API project
│       ├── Application/             # Business logic
│       ├── Domain/                  # Entities
│       └── Infrastructure/          # Data access
├── zalagaonica-web/
│   ├── Dockerfile
│   ├── .dockerignore
│   ├── nginx.conf                   # Frontend nginx config
│   ├── package.json
│   └── src/
│       ├── components/
│       ├── pages/
│       ├── services/
│       └── context/
├── nginx/
│   ├── nginx.conf                   # Main nginx config
│   └── conf.d/
│       └── zalagaonica.conf         # App routing config
├── docker-compose.yml               # Main compose file
├── docker-compose.prod.yml          # Production overrides
├── .env.example                     # Environment template
├── DEPLOYMENT.md                    # Detailed deployment guide
├── NGINX_ARCHITECTURE.md            # Nginx setup documentation
├── README.md                        # This file
├── CHECKLIST.md                     # Deployment checklist
├── start.sh                         # Start script
├── stop.sh                          # Stop script
├── backup.sh                        # Backup script
├── restore.sh                       # Restore script
└── logs.sh                          # Logs viewer script
```

## Implemented Features

### Backend (70% Complete)
- ✅ 40 Entity models
- ✅ 31 API controllers
- ✅ Full CRUD operations
- ✅ Authentication & authorization
- ✅ PDF report generation
- ✅ Database migrations
- ✅ Health checks
- ⚠️ Croatian bookkeeping integration (planned)

### Frontend (45% Complete)
- ✅ 27 functional pages
- ✅ Client management
- ✅ Article/inventory management
- ✅ Pledge operations
- ✅ Employee management
- ✅ Document management
- ✅ Modern UI with Tailwind
- ⚠️ Search functionality (partial)
- ⚠️ Advanced reporting (partial)

## Contributing

This is a closed-source application for internal use.

## License

Proprietary - All rights reserved

## Support

For deployment issues, see:
- `DEPLOYMENT.md` - Detailed deployment guide
- `NGINX_ARCHITECTURE.md` - Nginx configuration details
- `CHECKLIST.md` - Step-by-step checklist

For application issues, check:
1. Application logs: `./logs.sh`
2. Service status: `docker-compose ps`
3. Health endpoints: `curl http://localhost:3333/health`
4. Backend health: `curl http://localhost:3333/api/health`

---

**Built with ❤️ for Croatian pawn shop businesses**
