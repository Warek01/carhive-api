Remove-Item -Path .\Migrations -Recurse -Force
dotnet ef migrations add InitialMigration
dotnet ef database update
