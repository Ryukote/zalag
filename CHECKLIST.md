# Docker Deployment Checklist

## ✅ Files Created

All necessary Docker deployment files have been created:

### Core Docker Files
- [x] `backend/Dockerfile` - Backend .NET 8 container configuration
- [x] `zalagaonica-web/Dockerfile` - Frontend React container configuration
- [x] `docker-compose.yml` - Main orchestration file with health checks
- [x] `docker-compose.prod.yml` - Production-specific overrides

### Configuration Files
- [x] `.env.example` - Environment variables template
- [x] `zalagaonica-web/nginx.conf` - Nginx configuration for React SPA
- [x] `backend/.dockerignore` - Backend build exclusions
- [x] `zalagaonica-web/.dockerignore` - Frontend build exclusions
- [x] `zalagaonica-web/.env.production` - Frontend production env

### Application Files
- [x] `backend/.../Controllers/HealthController.cs` - Health check endpoints

### Management Scripts
- [x] `start.sh` - One-command startup script
- [x] `stop.sh` - Graceful shutdown script
- [x] `backup.sh` - Database backup with rotation
- [x] `restore.sh` - Database restore script
- [x] `logs.sh` - Log viewer script

### Documentation
- [x] `README.md` - Complete project overview
- [x] `DEPLOYMENT.md` - Detailed deployment guide
- [x] `CHECKLIST.md` - This file

## 📋 Pre-Deployment Checklist

Before copying to Linux server:

### 1. Review Configuration
- [ ] Review `.env.example` and understand all variables
- [ ] Check port mappings in `docker-compose.yml` (3000, 5000, 5432)
- [ ] Review resource limits in `docker-compose.prod.yml`

### 2. Security Review
- [ ] Plan strong database password
- [ ] Plan strong JWT secret (min 32 characters)
- [ ] Consider if database port (5432) should be exposed
- [ ] Review CORS policy in backend (currently AllowAll)

### 3. Network Planning
- [ ] Determine server IP or domain name
- [ ] Plan for HTTPS/SSL (reverse proxy setup)
- [ ] Determine if ports need to be changed
- [ ] Configure firewall rules

## 🚀 Deployment Steps

### 1. Server Preparation
```bash
# On your Linux server
sudo apt-get update
sudo apt-get install -y docker.io docker-compose curl
sudo systemctl enable docker
sudo systemctl start docker
```

### 2. Copy Files
```bash
# From your local machine
scp -r zalagaonica/ user@your-server:/home/user/

# Or using rsync (recommended)
rsync -avz --progress zalagaonica/ user@your-server:/home/user/zalagaonica/
```

### 3. Configure Environment
```bash
# On the server
cd /home/user/zalagaonica
cp .env.example .env
nano .env  # Edit all values!
```

**Critical `.env` values to set:**
```env
DB_PASSWORD=your_secure_password_here
JWT_SECRET=your_very_long_random_jwt_secret_key_here
REACT_APP_API_URL=http://your-server-ip:5000/api
```

### 4. Make Scripts Executable
```bash
chmod +x *.sh
```

### 5. Deploy
```bash
./start.sh
```

### 6. Verify Deployment
```bash
# Check service status
docker-compose ps

# All should show "Up (healthy)"

# Check logs
docker-compose logs -f

# Test endpoints
curl http://localhost:5000/api/health
curl http://localhost:3000/
```

### 7. Access Application
- Frontend: http://your-server-ip:3000
- Backend: http://your-server-ip:5000
- Swagger: http://your-server-ip:5000/swagger

### 8. First Login
- Email: `admin@pawnshop.hr`
- Password: `Admin123!`
- **⚠️ CHANGE PASSWORD IMMEDIATELY!**

## 🔒 Post-Deployment Security

### Immediate Actions
- [ ] Change admin password
- [ ] Set strong database password in `.env`
- [ ] Set strong JWT secret in `.env`
- [ ] Restrict database access (remove port 5432 exposure if not needed)

### Recommended Actions
- [ ] Set up HTTPS with Let's Encrypt
- [ ] Configure firewall (UFW)
- [ ] Set up automated backups (cron job)
- [ ] Set up monitoring (Prometheus/Grafana)
- [ ] Configure log rotation
- [ ] Set up uptime monitoring
- [ ] Review and restrict CORS policy
- [ ] Set up reverse proxy (nginx/Traefik)

## 🔧 Service Management

### Start Services
```bash
./start.sh
# or
docker-compose up -d
```

### Stop Services
```bash
./stop.sh
# or
docker-compose down
```

### View Logs
```bash
./logs.sh
# or
docker-compose logs -f [service-name]
```

### Restart Service
```bash
docker-compose restart backend
docker-compose restart frontend
```

### Update Application
```bash
# After code changes
docker-compose down
docker-compose up -d --build
```

## 💾 Backup Strategy

### Automated Backups
Add to crontab for daily backups:
```bash
crontab -e

# Add this line for daily backup at 2 AM
0 2 * * * cd /home/user/zalagaonica && ./backup.sh >> /var/log/zalagaonica-backup.log 2>&1
```

### Manual Backup
```bash
./backup.sh
```

### Restore from Backup
```bash
./restore.sh backups/zalagaonica_backup_YYYYMMDD_HHMMSS.sql.gz
```

## 🔍 Health Checks

### Service Health
```bash
# Overall health
curl http://localhost:5000/api/health

# Detailed readiness
curl http://localhost:5000/api/health/ready

# Liveness
curl http://localhost:5000/api/health/live

# Frontend
curl http://localhost:3000/

# Database
docker-compose exec database pg_isready -U postgres
```

### Container Status
```bash
docker-compose ps
# All should show "Up (healthy)"
```

## 🐛 Troubleshooting

### Services won't start
```bash
# Check logs
docker-compose logs

# Check specific service
docker-compose logs backend

# Restart all
docker-compose down && docker-compose up -d
```

### Database issues
```bash
# Check database logs
docker-compose logs database

# Restart database
docker-compose restart database

# Connect to database
docker-compose exec database psql -U postgres -d ZalagaonicaDB
```

### Frontend can't connect to backend
1. Check `REACT_APP_API_URL` in `.env`
2. Ensure it uses server IP or domain, not localhost
3. Rebuild: `docker-compose up -d --build frontend`

### "No space left on device"
```bash
# Clean Docker
docker system prune -a

# Check disk usage
df -h
docker system df
```

## 📊 Monitoring

### Resource Usage
```bash
# Real-time
docker stats

# Disk usage
docker system df
```

### Log Monitoring
```bash
# Follow logs
./logs.sh

# Specific service
./logs.sh backend
```

## 🎯 Service Dependencies

The startup order is enforced:

```
Database (PostgreSQL)
    ↓ (waits until healthy)
Backend (.NET API)
    ↓ (waits until healthy)
Frontend (React/Nginx)
```

Each service has health checks:
- **Database**: `pg_isready` check every 10s
- **Backend**: HTTP check on `/api/health` every 30s
- **Frontend**: HTTP check on root every 30s

## 📝 Important URLs

After deployment, bookmark these:

- **Application**: http://your-server-ip:3000
- **API**: http://your-server-ip:5000
- **API Docs**: http://your-server-ip:5000/swagger
- **Health Check**: http://your-server-ip:5000/api/health

## 💡 Tips

1. **Always test in development first** using `docker-compose up` (without -d) to see logs
2. **Keep backups** - Run `./backup.sh` before major changes
3. **Monitor logs** regularly with `./logs.sh`
4. **Update regularly** - Pull latest code and rebuild
5. **Security first** - Change default passwords, use HTTPS, firewall
6. **Document changes** - Keep notes of configuration changes

## 📞 Getting Help

1. Check logs: `./logs.sh`
2. Check service status: `docker-compose ps`
3. Check health endpoints
4. Review `DEPLOYMENT.md` for detailed troubleshooting
5. Check Docker and PostgreSQL documentation

## ✨ Success Criteria

Your deployment is successful when:

- ✅ All services show "Up (healthy)" in `docker-compose ps`
- ✅ Health endpoint returns 200: `curl http://localhost:5000/api/health`
- ✅ Frontend loads at http://your-server-ip:3000
- ✅ Swagger docs load at http://your-server-ip:5000/swagger
- ✅ You can login with admin credentials
- ✅ You can create/edit/view data
- ✅ Database persists after restart

## 🎉 You're Ready!

Once you've completed all the checklist items, your Zalagaonica application is ready for production use.

**Remember:**
- Start with `./start.sh`
- Stop with `./stop.sh`
- Backup with `./backup.sh`
- Monitor with `./logs.sh`

---

*For detailed information, see README.md and DEPLOYMENT.md*
