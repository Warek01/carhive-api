#!/bin/bash

# This script drops database and all generated migrations and apply a new one
# Use in development environment only

BASE='..'
PROJECT='FafCarsApi'
DB_HOST='localhost'
DB_USER='warek'
DB_NAME='faf_cars'
DEFAULT_DB_PASSWORD='warek'
export PGPASSWORD="${1:-$DEFAULT_DB_PASSWORD}"
RESET_SCHEMA_SQL=$(< ./reset_schema.sql)

rm -rf $BASE/Migrations/ && \
psql -U $DB_USER -h $DB_HOST -d $DB_NAME -w -c "$RESET_SCHEMA_SQL" && \
dotnet ef migrations add InitialMigration --project $BASE/$PROJECT.csproj && \
dotnet ef database update --project $BASE/$PROJECT.csproj && \
echo 'Database updated successfully.'
