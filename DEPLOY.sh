#!/bin/bash

echo "DEPLOYING ZALAGAONICA - CLEAN START"
echo "===================================="

cd /home/ftpuser/docker/zalagaonica/zalagaonica

# 1. Stop everything
echo "1. Stopping containers..."
docker compose down 2>/dev/null || true
docker stop $(docker ps -aq --filter "name=zalagaonica") 2>/dev/null || true
docker rm $(docker ps -aq --filter "name=zalagaonica") 2>/dev/null || true

# 2. Use the working config
echo "2. Using clean config..."
cp docker-compose-WORKING.yml docker-compose.yml

# 3. Start
echo "3. Starting (this takes 2-3 minutes)..."
docker compose up -d --build

# 4. Wait
echo "4. Waiting for startup..."
sleep 60

# 5. Open firewall
echo "5. Opening firewall..."
firewall-cmd --permanent --zone=public --add-port=3000/tcp 2>/dev/null || true
firewall-cmd --permanent --zone=public --add-port=3333/tcp 2>/dev/null || true
firewall-cmd --permanent --zone=public --add-port=5000/tcp 2>/dev/null || true
firewall-cmd --permanent --zone=public --add-port=5431/tcp 2>/dev/null || true
firewall-cmd --reload 2>/dev/null || true

# 6. Status
echo ""
echo "STATUS:"
echo "-------"
docker ps

echo ""
echo "TESTING:"
echo "--------"
curl -I http://localhost:3333 2>/dev/null | head -1
curl -I http://localhost:3000 2>/dev/null | head -1
curl -I http://localhost:5000/api/health 2>/dev/null | head -1

SERVER_IP=$(hostname -I | awk '{print $1}')
echo ""
echo "ACCESS FROM YOUR PC:"
echo "--------------------"
echo "http://$SERVER_IP:3333"
echo ""
