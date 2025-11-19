# Quick Start Guide - Nginx Setup (Port 3333)

## What Changed?

✅ **Nginx reverse proxy added** - Everything now runs through a single port (3333)
✅ **No CORS issues** - Frontend and backend on same origin
✅ **Production-ready** - Security headers, caching, compression built-in

## Architecture

```
                   Port 3333
                       │
                  ┌────▼────┐
                  │  Nginx  │
                  └────┬────┘
         ┌─────────────┼─────────────┐
         │                           │
    ┌────▼─────┐              ┌─────▼────┐
    │ Frontend │              │ Backend  │
    │ (React)  │              │ (.NET 8) │
    └──────────┘              └─────┬────┘
                                    │
                               ┌────▼────┐
                               │Database │
                               │(Postgres)│
                               └─────────┘
```

## One-Command Deploy

```bash
# Copy files to server
scp -r zalagaonica/ user@server:/home/user/

# On server
cd zalagaonica
cp .env.example .env
nano .env  # Edit: DB_PASSWORD, JWT_SECRET
./start.sh
```

## Access Application

**Single URL for everything:**
- 🌐 **Application**: http://your-server:3333
- 📡 **API**: http://your-server:3333/api
- 📚 **Swagger**: http://your-server:3333/swagger
- ❤️ **Health**: http://your-server:3333/health

## Login

```
Email: admin@pawnshop.hr
Password: Admin123!
```

⚠️ **Change password immediately!**

## Key Files

### `.env` Configuration
```env
DB_PASSWORD=your_strong_password
JWT_SECRET=your_32_char_random_key
NGINX_PORT=3333
REACT_APP_API_URL=/api
```

### New Files
- `nginx/nginx.conf` - Main nginx config
- `nginx/conf.d/zalagaonica.conf` - Routing rules
- `NGINX_ARCHITECTURE.md` - Detailed documentation

### Updated Files
- `docker-compose.yml` - Added nginx service
- `.env.example` - Updated for nginx
- `README.md` - Updated architecture docs

## Request Flow

### Frontend Request
```
Browser → http://server:3333/
   ↓
Nginx (port 3333)
   ↓
Frontend Container (port 80 internal)
   ↓
React App
```

### API Request
```
Browser → http://server:3333/api/articles
   ↓
Nginx (port 3333)
   ↓
Backend Container (port 80 internal)
   ↓
.NET API → Database
```

## Benefits

✅ **Single Port** - Only 3333 exposed
✅ **No CORS** - Same origin for frontend/backend
✅ **Fast** - Gzip compression, static caching
✅ **Secure** - Security headers, buffering
✅ **Easy SSL** - Add certificate to nginx
✅ **Scalable** - Ready for load balancing

## Service Status

```bash
docker-compose ps
```

Should show:
```
zalagaonica-db       Up (healthy)
zalagaonica-backend  Up (healthy)
zalagaonica-frontend Up (healthy)
zalagaonica-nginx    Up (healthy)
```

## Health Checks

```bash
# Nginx health
curl http://localhost:3333/health

# Backend health (through nginx)
curl http://localhost:3333/api/health

# Response:
# {"status":"Healthy","database":"Connected",...}
```

## Logs

```bash
# All services
./logs.sh

# Specific service
./logs.sh nginx
./logs.sh backend
./logs.sh frontend

# Or directly
docker-compose logs -f nginx
```

## Troubleshooting

### Can't access on port 3333
```bash
# Check nginx is running
docker-compose ps nginx

# Check nginx logs
docker-compose logs nginx

# Restart nginx
docker-compose restart nginx
```

### 502 Bad Gateway
```bash
# Backend or frontend not running
docker-compose ps

# Restart all
docker-compose restart
```

### Static files not loading
```bash
# Check browser console
# Check nginx access logs
docker-compose logs nginx | grep 404
```

## Customization

### Change Port
Edit `.env`:
```env
NGINX_PORT=8080
```

Restart:
```bash
docker-compose up -d nginx
```

### Add SSL/HTTPS
1. Get certificate (Let's Encrypt)
2. Mount in docker-compose.yml
3. Update nginx config
4. See `NGINX_ARCHITECTURE.md` for details

### Add Rate Limiting
Edit `nginx/conf.d/zalagaonica.conf`:
```nginx
limit_req_zone $binary_remote_addr zone=api:10m rate=10r/s;

location /api {
    limit_req zone=api burst=20;
    # ...
}
```

## Production Checklist

- [ ] Set strong `DB_PASSWORD`
- [ ] Set strong `JWT_SECRET` (32+ chars)
- [ ] Change admin password after first login
- [ ] Configure firewall (allow 22, 3333)
- [ ] Set up HTTPS with SSL certificate
- [ ] Set up automated backups (`./backup.sh`)
- [ ] Configure monitoring
- [ ] Review nginx logs regularly

## Backup & Restore

```bash
# Backup
./backup.sh

# Restore
./restore.sh backups/zalagaonica_backup_YYYYMMDD.sql.gz
```

## Useful Commands

```bash
# Start
./start.sh

# Stop
./stop.sh

# Logs
./logs.sh
./logs.sh nginx

# Restart service
docker-compose restart nginx

# Rebuild everything
docker-compose down
docker-compose up -d --build

# Check health
curl http://localhost:3333/health
curl http://localhost:3333/api/health
```

## Documentation

- `README.md` - Full project documentation
- `DEPLOYMENT.md` - Detailed deployment guide
- `NGINX_ARCHITECTURE.md` - Nginx configuration details
- `CHECKLIST.md` - Deployment checklist
- `QUICK_START.md` - This file

## Next Steps

1. ✅ Deploy application
2. ✅ Login and change password
3. ⚠️ Set up HTTPS (recommended)
4. ⚠️ Configure firewall
5. ⚠️ Set up backups
6. ⚠️ Configure monitoring

## Summary

You now have a production-ready deployment with:
- ✅ Single port (3333) for everything
- ✅ Nginx reverse proxy
- ✅ Automatic service orchestration
- ✅ Health checks
- ✅ No CORS issues
- ✅ Security headers
- ✅ Static asset caching
- ✅ Gzip compression

**Ready to go! Just run `./start.sh` 🚀**
