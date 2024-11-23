# Schnauz Project

## Overview

The Schnauz Project is a .NET-based web application that leverages multiple projects for different functionalities. It includes server-side components, client-side components, command handlers, and query handlers.

## Project Structure

- `Schnauz.Server`: The main server-side project.
- `Schnauz.Client`: The client-side project.
- `Schnauz.CommandHandlers`: Handles commands.
- `Schnauz.QueryHandlers`: Handles queries.
- `Schnauz.Shared`: Shared components and utilities.

## Prerequisites

- [.NET SDK 8.0](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker](https://www.docker.com/get-started)
- [Docker Compose](https://docs.docker.com/compose/install/)

## Getting Started

### Setting Up the Database

1. Start the PostgreSQL database using Docker Compose:

    ```sh
    docker-compose up -d
    ```

2. Ensure the database is running and accessible.

### Running the Application

1. Open a terminal and navigate to the `Schnauz.Server` directory.
2. Restore the dependencies:

    ```sh
    dotnet restore
    ```

3. Run the application:

    ```sh
    dotnet run --launch-profile "https"
    ```

### Configuration

- `appsettings.json`: General application settings.
- `appsettings.Development.json`: Development-specific settings, including database connection strings and logging levels.

## Project References

The `Schnauz.Server` project references other projects as follows:

```xml
<ItemGroup>
  <ProjectReference Include="..\Schnauz.Client\Schnauz.Client.csproj" />
  <ProjectReference Include="..\Schnauz.CommandHandlers\Schnauz.CommandHandlers.csproj" />
  <ProjectReference Include="..\Schnauz.QueryHandlers\Schnauz.QueryHandlers.csproj" />
</ItemGroup>