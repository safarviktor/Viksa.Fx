# Viksa.Fx

## Prerequisities

### SQL backend

Some features require a MS SQL Server database to work. When you set up the database:
- run the scripts in folder "_database"
- update `_ConnectionString` in `Viksa.Fx.DataAccess.BaseRepository`

### Fixer.io

Some features require the `Fixer.io` hookup. Update the `ApiKey` in `Viksa.Fx.FixerClient.Client`.
