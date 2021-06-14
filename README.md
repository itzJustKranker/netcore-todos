# .Net Core Todos

.Net Core Todos is a simple web api that stores todo lists and todo items.

## Setup

### Required Tools

1. In order to build and run this project, you will need to have the .Net Core 3.1 SDK installed.
If you do not have this SDK installed locally already, you can download the installer [here](https://dotnet.microsoft.com/download/dotnet/3.1).
Be sure to select the appropriate installer for your operating system.

2. You will also need the EntityFramework Core tools installed globally to work with the database you will
   setup in the next steps. Once you have the dotnet SDK CLI installed from above, go ahead and 
   run this command from a terminal (i.e. Powershell, CMD, Bash, etc.)
   
```shell
$ dotnet tool install --global dotnet-ef
```

3. This application requires a SQL Server instance to store application data, if you are running
on windows, you can install [SQL Server Express](https://www.microsoft.com/en-us/sql-server/sql-server-downloads). If you are running on macOS or Linux, or you are on Windows but prefer 
to work with containers, you will need to setup [docker](https://docs.docker.com/get-docker/) for your operating system. Then pull and configure the official Microsoft 
SQL Server on Linux container which can be found [here](https://hub.docker.com/_/microsoft-mssql-server).

### Database Configuration

4. Once your preferred SQL Server instance is installed and running, make sure you create a database that you can use to work with this application. 
The database name can be whatever you choose, however make sure you are pointing to the
correct database you created when you define your connection string later on.

5. If you are running this project locally, you should create a new file in the `src/Todos.WebUI` project named `appsettings.development.json`. 
You should copy over the contents from the example `appsettings.json` file. 

> Make sure you do not include the comment at the top of the file indicating that `appsettings.json` 
is meant to act as a blank example file, not to store any setting values.

6. You will need to provide a connection string to the SQL Server instance you setup within `appsettings.development.json`.
This connection string should be placed within the designated setting slot `ConnectionStrings:DefaultConnectionString: ""`.

7. After you have defined your SQL Server instance connection string, you are ready to run the migrations
that will ensure your database is configured to work with this application. From a terminal, `cd` into your projects root directory (i.e. ~/netcore-todos)

    1. `cd` into the `Todos.WebUI` project
    
    ```shell
    $ cd src/Todos.WebUI 
    ```

    2. from the `Todos.WebUI` project, run the following command. Ensure that your SQL Server instance is running, and you have added the proper
    connection string to your `appsettings.development.json` file

    ```shell
    $ dotnet ef database update -p "../Todos.Infrastructure"
    ```
    
This should build the project and apply the migrations to the database you have created and set in your
connection string.

## Run

If you have completed the setup steps above, you are now ready to build and run the application.

> Ensure that your SQL Server instance is running and you have defined the correct connection string within the 
> `appsettings.development.json` file before attempting to start the application.

The projects run configurations are defined within `launchSettings.json` in the `src/Todos.WebUI/Properties` directory. 
You should target your IDE to run the `Todos.WebUI` profile. If you are using Visual Studio, you should set the startup
project as `Todos.WebUI`.

If you are not using an IDE, the project can also be started from the command line.
From within the `Todos.WebUI` directory, run the following command to start the application.

```shell
$ dotnet run
```
