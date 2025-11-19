#!/bin/bash
# Zalagaonica - Database Backup Script

set -e

BACKUP_DIR="./backups"
TIMESTAMP=$(date +%Y%m%d_%H%M%S)
BACKUP_FILE="$BACKUP_DIR/zalagaonica_backup_$TIMESTAMP.sql.gz"

echo "======================================"
echo "Zalagaonica Database Backup"
echo "======================================"
echo ""

# Create backup directory if it doesn't exist
mkdir -p "$BACKUP_DIR"

# Check if database container is running
if ! docker-compose ps database | grep -q "Up"; then
    echo "❌ Database container is not running"
    echo "Start it with: docker-compose up -d database"
    exit 1
fi

echo "📦 Creating database backup..."
echo "Backup file: $BACKUP_FILE"
echo ""

# Create backup
docker-compose exec -T database pg_dump -U postgres ZalagaonicaDB | gzip > "$BACKUP_FILE"

# Check if backup was successful
if [ -f "$BACKUP_FILE" ]; then
    BACKUP_SIZE=$(du -h "$BACKUP_FILE" | cut -f1)
    echo "✅ Backup created successfully!"
    echo "   File: $BACKUP_FILE"
    echo "   Size: $BACKUP_SIZE"
    echo ""

    # Keep only last 7 backups
    echo "🧹 Cleaning up old backups (keeping last 7)..."
    cd "$BACKUP_DIR"
    ls -t zalagaonica_backup_*.sql.gz | tail -n +8 | xargs -r rm
    cd ..

    echo "✅ Cleanup complete"
    echo ""
    echo "To restore this backup:"
    echo "  ./restore.sh $BACKUP_FILE"
else
    echo "❌ Backup failed!"
    exit 1
fi

echo ""
