#!/bin/bash
# Zalagaonica - Stop Script

set -e

echo "======================================"
echo "Stopping Zalagaonica Application"
echo "======================================"
echo ""

# Check if services are running
if ! docker-compose ps | grep -q "Up"; then
    echo "ℹ️  No services are currently running"
    exit 0
fi

echo "🛑 Stopping all services..."
docker-compose down

echo ""
echo "✅ All services stopped successfully"
echo ""
echo "To start again, run: ./start.sh"
echo "To remove all data, run: docker-compose down -v"
echo ""
