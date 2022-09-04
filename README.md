# Find my lambda

## Marketing pitch
Were you ever in need of a function but couldn't find it anywhere while searching?

I have the solution for you!

Find my lambda lets you upload your function to the database in several different languages and lets you later find what function you are looking for by giving it some pairs of inputs/ outputs. It also combines multiple functions to find your desired lambda.



## Description

There are four parts to this project.
1. The language servers
    - These are the only pieces in the system that run the submited code. They need to be isolated if the environment is shared with untrusted parties.
    - Planned languages:
        - C# ✅
        - Dart 🕛
        - F# 🕛
        - Java 🕛
        - Python 🕛
2. The actual server
    - A server written in ASP.net that handles the actual searching of the functions.
    - It also interacts with the database and handles caching results and combining functions to FOGs.
3. The Database
    - Simple microsoft sql database that is run in a docker container.
    - To run it follow the tutorial at the Database section.
4. The front-end.
    - Currently written in Dart using Flutter to enable running the app in desktop and web mode.



## The database installation

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

Download azure data studio
https://docs.microsoft.com/en-us/sql/azure-data-studio/download-azure-data-studio?view=sql-server-ver16

Run the SqlSetup.ipynb found in the SQL folder of the root directory.
