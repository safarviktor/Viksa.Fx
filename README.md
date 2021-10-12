# Viksa.Fx

## Prerequisities

### SQL backend

Some features require a MS SQL Server database to work. After you've set up the database:
- run script 001 in folder "_database"
- update `_ConnectionString` in `Viksa.Fx.DataAccess.BaseRepository`

### Fixer.io

Some features require the `Fixer.io` hookup. Update the `ApiKey` in `Viksa.Fx.FixerClient.Client`.

### Demo website

To get a reasonable number of data points to show on the graph, the 002 SQL script was created. It is dependent on having at least one set of rates in the database - so it is best to run the 002 script after the "daily" import has been run at least once.
