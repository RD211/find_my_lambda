Download SQL Server
```
docker pull mcr.microsoft.com/mssql/server
```


Run SQL Server
```
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=<PASS HERE>" -p 1433:1433 --name sql1 --hostname sql1 -d mcr.microsoft.com/mssql/server:2022-latest
```

```
dotnet user-secrets init
```

```
dotnet user-secrets set "Database:DataSource" "localhost"
dotnet user-secrets set "Database:UserId" "SA"
dotnet user-secrets set "Database:Password" "<PASS HERE>"
dotnet user-secrets set "Database:Catalog" "master"
```