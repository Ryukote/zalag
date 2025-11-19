#!/bin/bash
# Zalagaonica - View Logs Script

SERVICE="${1:-all}"

echo "======================================"
echo "Zalagaonica Application Logs"
echo "======================================"
echo ""

if [ "$SERVICE" = "all" ]; then
    echo "📋 Showing logs for all services (press Ctrl+C to exit)"
    echo ""
    docker-compose logs -f
else
    echo "📋 Showing logs for: $SERVICE (press Ctrl+C to exit)"
    echo ""
    docker-compose logs -f "$SERVICE"
fi
