# Nginx Reverse Proxy Architecture

## Overview

The Zalagaonica application uses nginx as a reverse proxy to expose both frontend and backend services through a **single port (3333)**. This provides several benefits:

- ✅ Single entry point for the entire application
- ✅ No CORS issues (frontend and backend on same origin)
- ✅ Centralized SSL/TLS termination (when configured)
- ✅ Static asset caching
- ✅ Load balancing capability (future)
- ✅ Security headers
- ✅ Request buffering and compression

## Architecture Diagram

```
                                    Port 3333
                                       │
                                       ▼
┌──────────────────────────────────────────────────────────────┐
│                      Nginx Reverse Proxy                      │
│                   (zalagaonica-nginx)                        │
│                                                              │
│  Routes:                                                     │
│  • / → Frontend                                             │
│  • /api/* → Backend                                         │
│  • /swagger → Backend (API Docs)                            │
│  • /health → Nginx health check                             │
└──────────────────────────────────────────────────────────────┘
        │                                    │
        │ proxy_pass                        │ proxy_pass
        ▼                                    ▼
┌──────────────────┐              ┌──────────────────┐
│    Frontend      │              │     Backend      │
│  (React + Nginx) │              │   (.NET 8 API)   │
│   Port 80        │              │    Port 80       │
│   (internal)     │              │   (internal)     │
└──────────────────┘              └──────────────────┘
                                           │
                                           │
                                           ▼
                                  ┌──────────────────┐
                                  │    Database      │
                                  │   (PostgreSQL)   │
                                  │   Port 5432      │
                                  │   (internal)     │
                                  └──────────────────┘
```

## Service Startup Order

```
1. Database
      ↓ (health: pg_isready)
2. Backend
      ↓ (health: /api/health)
3. Frontend
      ↓ (health: HTTP 200)
4. Nginx
      ✓ (health: /health)
```

## Port Mapping

### External (Exposed to Host)
- **3333** → Nginx (only exposed port)

### Internal (Docker Network Only)
- Frontend: 80 (not exposed to host)
- Backend: 80 (not exposed to host)
- Database: 5432 (optionally exposed for dev/admin)

## Request Flow

### Frontend Requests
```
Browser → http://your-server:3333/
         ↓
      Nginx
         ↓ proxy_pass to frontend:80
    Frontend Container
         ↓ serves React SPA
      Browser
```

### API Requests
```
Browser → http://your-server:3333/api/articles
         ↓
      Nginx
         ↓ proxy_pass to backend:80/api/articles
    Backend Container
         ↓ processes request
         ↓ queries database
      Returns JSON
```

### Swagger Documentation
```
Browser → http://your-server:3333/swagger
         ↓
      Nginx
         ↓ proxy_pass to backend:80/swagger
    Backend Container
         ↓ serves Swagger UI
      Browser
```

## Nginx Configuration Files

### `/nginx/nginx.conf`
Main nginx configuration:
- Worker processes
- Gzip compression
- Buffer sizes
- Logging
- Global settings

### `/nginx/conf.d/zalagaonica.conf`
Application-specific configuration:
- Upstream definitions (frontend, backend)
- Routing rules
- Proxy settings
- Security headers
- Caching rules

## Routing Rules

| URL Pattern | Destination | Cache |
|------------|-------------|-------|
| `/api/*` | Backend API | No |
| `/swagger` | Backend Swagger | No |
| `/health` | Nginx health | No |
| `/*.js`, `/*.css` | Frontend static | 1 year |
| `/*.png`, `/*.jpg` | Frontend static | 1 year |
| `/*` (all others) | Frontend SPA | No |

## Security Headers

Nginx automatically adds these security headers:

```nginx
X-Frame-Options: SAMEORIGIN
X-Content-Type-Options: nosniff
X-XSS-Protection: 1; mode=block
Referrer-Policy: no-referrer-when-downgrade
```

## Caching Strategy

### Static Assets (1 year cache)
- JavaScript files (.js)
- CSS files (.css)
- Images (.jpg, .jpeg, .png, .gif, .svg, .ico)
- Fonts (.woff, .woff2, .ttf, .eot)

Headers: `Cache-Control: public, immutable`

### HTML Files (no cache)
- index.html and all HTML files
- Ensures users always get latest version

Headers: `Cache-Control: no-cache, no-store, must-revalidate`

### API Responses (no cache by default)
- Backend controls caching via response headers

## Health Checks

### Nginx Health Check
```bash
curl http://localhost:3333/health
# Response: "healthy"
```

### Backend Health (through nginx)
```bash
curl http://localhost:3333/api/health
# Response: {"status": "Healthy", "database": "Connected", ...}
```

### Frontend Health (through nginx)
```bash
curl http://localhost:3333/
# Response: HTML content
```

## Customization

### Change Port
Edit `.env`:
```env
NGINX_PORT=8080
```

### Add SSL/HTTPS
1. Obtain SSL certificate (Let's Encrypt, etc.)
2. Mount certificates in docker-compose.yml:
```yaml
nginx:
  volumes:
    - ./nginx/nginx.conf:/etc/nginx/nginx.conf:ro
    - ./nginx/conf.d:/etc/nginx/conf.d:ro
    - ./ssl:/etc/nginx/ssl:ro  # Add this
```
3. Update `nginx/conf.d/zalagaonica.conf`:
```nginx
server {
    listen 443 ssl http2;
    ssl_certificate /etc/nginx/ssl/cert.pem;
    ssl_certificate_key /etc/nginx/ssl/key.pem;
    # ... rest of config
}
```

### Add Rate Limiting
Add to `nginx/conf.d/zalagaonica.conf`:
```nginx
# Define rate limit zone
limit_req_zone $binary_remote_addr zone=api_limit:10m rate=10r/s;

# Apply to location
location /api {
    limit_req zone=api_limit burst=20;
    # ... rest of config
}
```

### Add Basic Authentication
```bash
# Create password file
htpasswd -c nginx/htpasswd admin

# Add to nginx config
location / {
    auth_basic "Restricted";
    auth_basic_user_file /etc/nginx/htpasswd;
    # ... rest of config
}
```

## Troubleshooting

### 502 Bad Gateway
```bash
# Check if backend/frontend are healthy
docker-compose ps

# Check nginx logs
docker-compose logs nginx

# Check backend logs
docker-compose logs backend
```

### 504 Gateway Timeout
- Backend is taking too long to respond
- Increase timeout in nginx config:
```nginx
proxy_read_timeout 120s;
```

### Static files not loading
- Check browser console for 404 errors
- Verify nginx routing rules
- Check nginx access logs:
```bash
docker-compose exec nginx tail -f /var/log/nginx/access.log
```

### CORS errors (shouldn't happen)
- With this setup, CORS shouldn't be an issue
- Frontend and backend are on same origin
- If you see CORS errors, check browser console for actual issue

## Monitoring

### Access Logs
```bash
docker-compose exec nginx tail -f /var/log/nginx/access.log
```

### Error Logs
```bash
docker-compose exec nginx tail -f /var/log/nginx/error.log
```

### Real-time Stats
```bash
# Install nginx-module-vts for stats (optional)
# Or use nginx stub_status module
```

## Benefits of This Architecture

1. **Single Port**: Only one port to expose/manage
2. **No CORS**: Frontend and backend on same origin
3. **Centralized Security**: All requests go through nginx
4. **Performance**: Static asset caching, gzip compression
5. **Flexibility**: Easy to add SSL, rate limiting, auth
6. **Scalability**: Can add load balancing easily
7. **Monitoring**: Centralized access/error logs

## Performance Optimizations

Already configured:
- ✅ Gzip compression for text assets
- ✅ Static asset caching (1 year)
- ✅ Keepalive connections
- ✅ TCP optimizations (tcp_nopush, tcp_nodelay)
- ✅ Buffering for proxy requests
- ✅ Worker process auto-scaling

## Security Best Practices

Implemented:
- ✅ Security headers
- ✅ Limited buffer sizes
- ✅ No server version disclosure
- ✅ Internal services not exposed
- ✅ Timeout protections

Recommended additions:
- ⚠️ SSL/TLS encryption
- ⚠️ Rate limiting
- ⚠️ Fail2ban integration
- ⚠️ ModSecurity WAF
- ⚠️ IP whitelisting (if applicable)

## Conclusion

This nginx reverse proxy setup provides a production-ready, secure, and performant architecture for the Zalagaonica application with minimal configuration required.
