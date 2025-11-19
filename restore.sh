#!/bin/bash
# Zalagaonica - Database Restore Script

set -e

if [ -z "$1" ]; then
    echo "Usage: ./restore.sh <backup-file>"
    echo ""
    echo "Available backups:"
    ls -lh backups/*.sql.gz 2>/dev/null || echo "  No backups found"
    exit 1
fi

BACKUP_FILE="$1"

if [ ! -f "$BACKUP_FILE" ]; then
    echo "❌ Backup file not found: $BACKUP_FILE"
    exit 1
fi

echo "======================================"
echo "Zalagaonica Database Restore"
echo "======================================"
echo ""
echo "⚠️  WARNING: This will replace all current data!"
echo "Backup file: $BACKUP_FILE"
echo ""
read -p "Are you sure you want to continue? (yes/no): " CONFIRM

if [ "$CONFIRM" != "yes" ]; then
    echo "Restore cancelled"
    exit 0
fi

echo ""
echo "🛑 Stopping backend service..."
docker-compose stop backend

echo "📦 Restoring database..."
gunzip < "$BACKUP_FILE" | docker-compose exec -T database psql -U postgres ZalagaonicaDB

echo "🚀 Starting backend service..."
docker-compose start backend

echo ""
echo "✅ Database restored successfully!"
echo ""
