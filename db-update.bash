rm -rf ./Migrations && \
PGPASSWORD=warek psql -U warek -h localhost -d faf_cars -w -c \
'DROP TABLE IF EXISTS  users CASCADE; DROP TABLE IF EXISTS listings CASCADE; DROP TABLE IF EXISTS  "__EFMigrationsHistory" CASCADE;' && \
dotnet ef migrations add InitialMigration && \
dotnet ef database update && \
echo Database updated successfully.
