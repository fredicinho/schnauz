# Schnauz Project

## Overview

The Schnauz Project is a .NET-based web application that leverages multiple projects for different functionalities. It includes server-side components, client-side components, command handlers, and query handlers.

## Project Structure

- `Schnauz.Server`: The main server-side project, which includes the Frontend, Backend and Grain Client.
- `Schnauz.Client`: The frontend.
- `Schnauz.CommandHandlers`: Handles commands (API).
- `Schnauz.QueryHandlers`: Handles queries (API).
- `Schnauz.Shared`: Shared components and utilities between frontend and backend.
- `Schnauz.Grains`: The Grain implementations.
- `Schnauz.GrainInterfaces`: The interfaces for the Grains.
- `Schnauz.Silo`: The Silo Server.

## Prerequisites

- [.NET SDK 8.0](https://dotnet.microsoft.com/download/dotnet/8.0)

## Getting Started

### Running the Application

1. Open a terminal.
2. Restore the dependencies:

    ```sh
    dotnet restore
    ```

3. Run the applications (Silo Server and Server). Run the following commands in separate terminals:

    ```sh
    dotnet run --launch-profile "https" --project Schnauz.Server
    dotnet run --project Schnauz.Server
    ```