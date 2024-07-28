#!/bin/bash

# This script drops database and all generated migrations and apply a new one
# Use in development environment only

# print command on execution
set -x

PROJECT=FafCarsApi.csproj
MIGRATIONS_DIR=Data/Migrations/
CONTEXT=FafCarsApi.Data.FafCarsDbContext

if [ -e "./$PROJECT" ]; then
  echo "Project file found in current directory."
elif [ -e "../$PROJECT" ]; then
  echo "Project file found in parent directory."
  cd ..
else
  echo "Error: Project file not found in current or parent directory."
  exit 1
fi

rm -rf $MIGRATIONS_DIR &&
dotnet ef database drop --force --startup-project $PROJECT --project $PROJECT &&
dotnet ef migrations add InitialMigration --project $PROJECT --output-dir $MIGRATIONS_DIR --context $CONTEXT &&
dotnet ef database update --project $PROJECT &&
echo 'Database updated successfully.'
