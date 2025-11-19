#!/bin/bash
# Zalagaonica - Quick Start Script

set -e

echo "======================================"
echo "Zalagaonica Application - Quick Start"
echo "======================================"
echo ""

# Check if Docker is installed
if ! command -v docker &> /dev/null; then
    echo "❌ Docker is not installed. Please install Docker first."
    echo "Visit: https://docs.docker.com/engine/install/"
    exit 1
fi

# Check if Docker Compose is installed
if ! command -v docker-compose &> /dev/null; then
    echo "❌ Docker Compose is not installed. Please install Docker Compose first."
    echo "Visit: https://docs.docker.com/compose/install/"
    exit 1
fi

echo "✅ Docker and Docker Compose are installed"
echo ""

# Check if .env file exists
if [ ! -f .env ]; then
    echo "⚠️  .env file not found. Creating from .env.example..."
    cp .env.example .env
    echo "✅ .env file created"
    echo ""
    echo "⚠️  IMPORTANT: Please edit .env file and set your configuration!"
    echo "Run: nano .env"
    echo ""
    read -p "Press Enter to continue after editing .env file, or Ctrl+C to exit..."
fi

echo "📦 Starting services..."
echo ""

# Stop any existing containers
docker-compose down

# Pull latest images (if any are from registry)
# docker-compose pull

# Build and start services
docker-compose up -d --build

echo ""
echo "⏳ Waiting for services to be healthy..."
echo ""

# Wait for services to be healthy
sleep 5

# Check service status
for i in {1..30}; do
    if docker-compose ps | grep -q "Up (healthy)"; then
        echo "✅ Services are starting up..."
        break
    fi
    echo "   Waiting... ($i/30)"
    sleep 2
done

echo ""
echo "======================================"
echo "📊 Service Status:"
echo "======================================"
docker-compose ps
echo ""

echo "======================================"
echo "🎉 Deployment Complete!"
echo "======================================"
echo ""
echo "Access your application:"
echo "  Frontend:  http://localhost:3000"
echo "  Backend:   http://localhost:5000"
echo "  Swagger:   http://localhost:5000/swagger"
echo ""
echo "Default Admin Credentials:"
echo "  Email:     admin@pawnshop.hr"
echo "  Password:  Admin123!"
echo ""
echo "⚠️  IMPORTANT: Change the admin password immediately!"
echo ""
echo "Useful commands:"
echo "  View logs:        docker-compose logs -f"
echo "  Stop services:    docker-compose down"
echo "  Restart services: docker-compose restart"
echo ""
echo "For more information, see DEPLOYMENT.md"
echo ""
