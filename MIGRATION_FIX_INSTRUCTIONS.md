# Migration Fix Instructions for CentOS Server

## Problem
The error "Column 'Status' cannot be cast automatically to type integer" occurs because:
1. The old migration created Status as `varchar(30)` (string)
2. The entity expects Status as `integer` (enum)
3. PostgreSQL cannot automatically convert existing string data to integer

## Root Cause Analysis

### Files Analyzed:
1. **Vacation.cs** (line 36): Status is correctly defined as `VacationStatus` enum
2. **ApplicationDbContext.cs**: No Fluent API override for Vacation (uses default conventions)
3. **20251116233940_AddInventoryEntities.cs** (line 178): **FIXED** - Creates Status as integer
4. **20251117070414_AddVacationTypeField.cs**: **FIXED** - Removed AlterColumn logic
5. **ApplicationDbContextModelSnapshot.cs**: Shows Status as integer (correct)

### Current State on Windows Dev Machine:
✅ Migration files are correctly fixed
✅ Entity definition is correct
✅ No conflicting Fluent API configurations

### Problem on CentOS Server:
❌ Old database volume exists with Status as string
❌ Fixed migration files may not be uploaded to server

## Solution Steps

### Step 1: Upload Fixed Migration Files to Server

Upload these two files to `/home/ftpuser/docker/zalagaonica/zalagaonica/backend/Zalagaonica.Backend/Infrastructure/Migrations/`:

1. **20251116233940_AddInventoryEntities.cs**
2. **20251117070414_AddVacationTypeField.cs**

### Step 2: Stop and Remove All Containers

```bash
cd /home/ftpuser/docker/zalagaonica/zalagaonica
docker compose down
```

### Step 3: Remove the Old Database Volume

**CRITICAL STEP** - This removes ALL existing data:

```bash
docker volume rm zalagaonica-postgres-data
```

If you get "volume is in use" error:
```bash
docker ps -a  # List all containers
docker rm -f <container_id>  # Remove any remaining containers
docker volume rm zalagaonica-postgres-data
```

### Step 4: Verify Volume is Removed

```bash
docker volume ls | grep zalagaonica
```

Should show NO volumes.

### Step 5: Rebuild and Start

```bash
docker compose up --build -d
```

### Step 6: Monitor the Logs

```bash
docker logs zalagaonica-backend -f
```

Look for:
- ✅ "Applying migration '20251116233940_AddInventoryEntities'"
- ✅ "Applying migration '20251117070414_AddVacationTypeField'"
- ✅ No errors about column type casting

## Verification

After successful startup, verify the database schema:

```bash
# Connect to PostgreSQL container
docker exec -it zalagaonica-db psql -U postgres -d ZalagaonicaDB

# Check Vacations table schema
\d "Vacations"
```

Expected output for Status column:
```
Status | integer | not null | default 0
```

## Why This Happens

1. **First deployment**: Migration created Status as `varchar(30)` (old/incorrect code)
2. **Database persists**: Docker volume keeps the old schema even after code changes
3. **Fixed migration can't run**: PostgreSQL won't auto-convert existing string→integer
4. **Solution**: Remove volume so fresh database is created with correct schema

## Prevention

For future migrations that change column types, always:
1. Create a new migration with proper conversion logic
2. Or remove the database volume if in development (loses all data)
3. Never try to "fix" old migrations after they've been applied to production

## Alternative Solution (If Data Must Be Preserved)

If you need to keep existing data, create a NEW migration:

```bash
cd /home/ftpuser/docker/zalagaonica/zalagaonica/backend/Zalagaonica.Backend
dotnet ef migrations add FixVacationStatusType
```

Then manually edit the new migration to include conversion logic:

```csharp
protected override void Up(MigrationBuilder migrationBuilder)
{
    migrationBuilder.Sql(@"
        ALTER TABLE ""Vacations""
        ALTER COLUMN ""Status"" TYPE integer
        USING CASE
            WHEN ""Status"" = 'Pending' THEN 0
            WHEN ""Status"" = 'Approved' THEN 1
            WHEN ""Status"" = 'Rejected' THEN 2
            ELSE 0
        END;
    ");
}
```

But since this is a fresh deployment with no production data, **removing the volume is the cleanest solution**.
