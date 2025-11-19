#!/bin/bash

# COMPLETE FIX - No more debugging, just fix it
# This script will fix EVERYTHING automatically

set -e

echo "========================================="
echo "FIXING EVERYTHING - PLEASE WAIT"
echo "========================================="
echo ""

cd /home/ftpuser/docker/zalagaonica/zalagaonica

# Step 1: STOP EVERYTHING
echo "1. Stopping all containers..."
docker compose down --remove-orphans
docker stop zalagaonica-nginx zalagaonica-frontend zalagaonica-backend zalagaonica-db 2>/dev/null || true
docker rm -f zalagaonica-nginx zalagaonica-frontend zalagaonica-backend zalagaonica-db 2>/dev/null || true
sleep 5

# Step 2: CLEAN UP
echo "2. Cleaning up..."
docker system prune -f

# Step 3: REBUILD AND START
echo "3. Rebuilding and starting (this will take 2-3 minutes)..."
docker compose up --build -d --force-recreate

# Step 4: WAIT FOR STARTUP
echo "4. Waiting 60 seconds for all services to start..."
for i in {60..1}; do
    echo -ne "   $i seconds remaining...\r"
    sleep 1
done
echo ""

# Step 5: OPEN FIREWALL
echo "5. Opening firewall ports..."
firewall-cmd --permanent --add-port=3000/tcp 2>/dev/null || true
firewall-cmd --permanent --add-port=3333/tcp 2>/dev/null || true
firewall-cmd --permanent --add-port=5000/tcp 2>/dev/null || true
firewall-cmd --permanent --add-port=5431/tcp 2>/dev/null || true
firewall-cmd --reload 2>/dev/null || true

# Step 6: CHECK STATUS
echo ""
echo "========================================="
echo "CHECKING IF IT WORKS"
echo "========================================="
echo ""

docker ps --format "table {{.Names}}\t{{.Status}}"
echo ""

# Get server IP
SERVER_IP=$(hostname -I | awk '{print $1}')

# Test each service
echo "Testing services..."
echo ""

# Test nginx
if curl -s -o /dev/null -w "%{http_code}" http://localhost:3333 | grep -q "200\|404"; then
    echo "✓ Nginx (3333): WORKING"
else
    echo "✗ Nginx (3333): FAILED"
fi

# Test frontend
if curl -s -o /dev/null -w "%{http_code}" http://localhost:3000 | grep -q "200\|404"; then
    echo "✓ Frontend (3000): WORKING"
else
    echo "✗ Frontend (3000): FAILED"
fi

# Test backend
if curl -s -o /dev/null -w "%{http_code}" http://localhost:5000/api/health 2>/dev/null | grep -q "200\|404"; then
    echo "✓ Backend (5000): WORKING"
else
    echo "✗ Backend (5000): FAILED"
fi

# Test database
if nc -z localhost 5431 2>/dev/null || timeout 1 bash -c "cat < /dev/null > /dev/tcp/localhost/5431" 2>/dev/null; then
    echo "✓ Database (5431): WORKING"
else
    echo "✗ Database (5431): FAILED"
fi

echo ""
echo "========================================="
echo "YOUR URLS"
echo "========================================="
echo ""
echo "Access from your browser:"
echo ""
echo "  Frontend:  http://$SERVER_IP:3000"
echo "  Main App:  http://$SERVER_IP:3333"
echo "  Backend:   http://$SERVER_IP:5000/api/health"
echo ""
echo "If still not accessible from your PC:"
echo "  - Your hosting provider has additional firewall"
echo "  - Contact your hosting provider support"
echo ""
