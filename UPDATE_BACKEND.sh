#!/bin/bash

echo "========================================="
echo "UPDATING BACKEND WITH FIXES"
echo "========================================="
echo ""

cd /home/ftpuser/docker/zalagaonica/zalagaonica

echo "1. Stopping backend container..."
docker compose stop backend

echo ""
echo "2. Rebuilding backend..."
docker compose build backend

echo ""
echo "3. Starting backend..."
docker compose up -d backend

echo ""
echo "4. Waiting for backend to start..."
sleep 15

echo ""
echo "5. Checking backend health..."
curl -I http://localhost:5000/api/health

echo ""
echo "6. Testing Article endpoint..."
echo "Attempting to fetch articles..."
curl -s http://localhost:5000/api/Article | head -20

echo ""
echo ""
echo "========================================="
echo "UPDATE COMPLETE!"
echo "========================================="
echo ""
echo "Article creation should now work!"
echo "Test it from your frontend."
echo ""
