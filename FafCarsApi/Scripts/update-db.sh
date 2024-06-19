#!/bin/bash

# This script drops database and all generated migrations and apply a new one
# Use in development environment only

# errexit, exit on error
set -e
# print command on execution
set -x

export BASE='..'
export PROJECT='FafCarsApi'
export DB_HOST='localhost'
export DB_USER='warek'
export DB_NAME='faf_cars'
DEFAULT_DB_PASSWORD='warek'
export PGPASSWORD="${1:-$DEFAULT_DB_PASSWORD}"

rm -rf $BASE/Migrations/
dotnet ef database drop --force --verbose --startup-project $BASE/$PROJECT.csproj --project $BASE/$PROJECT.csproj
dotnet ef migrations add InitialMigration --project $BASE/$PROJECT.csproj
dotnet ef database update --project $BASE/$PROJECT.csproj
echo 'Database updated successfully.'
