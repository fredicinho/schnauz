# Schnauz
A multiplayer card game implemented using the Actor Model with Orleans. Check out the following link to learn more about the game: [Schnauz](https://en.wikipedia.org/wiki/Schwimmen).

![Alt Text](images/record.gif)

## Overview


## Project Structure

- `Schnauz.Server`: The main server-side project, which includes the Frontend, Backend and Grain Client.
- `Schnauz.Client`: The frontend.
- `Schnauz.Shared`: Shared components and utilities between frontend, backend and silo.
- `Schnauz.Grains`: The Grain implementations.
- `Schnauz.GrainInterfaces`: The interfaces for the Grains.
- `Schnauz.Silo`: The Silo Server.

## Prerequisites

- [Docker](https://www.docker.com/products/docker-desktop)
- [.NET SDK 8.0](https://dotnet.microsoft.com/download/dotnet/8.0)

## Getting Started

### Running the Application

1. Open a terminal.
2. Start redis and jaeger with docker-compose:

    ```sh
    docker-compose up
    ```
3. Restore the dependencies:

    ```sh
    dotnet restore
    ```

4. Run the applications (Silo Server and Server). Run the following commands in separate terminals:

    ```sh
    dotnet run --project Schnauz.Silo
    dotnet run --launch-profile "https" --project Schnauz.Server
    ```
   
You can choose to run the application without https by using the "http" launch profile.

## System Architecture
![Alt text](images/schnauz.drawio.png)


## Actor Model Architecture
![Alt text](images/actor-architecture.drawio.png)

## Architectural Decisions
### Same Grain ID of Match and CardDealer
Both grains use the same ID as they have a one-to-one relationship.

## Challenges during the Project

### Deadlocks through circular calls of grains
The first challenge we faced was the circular calls of grains. We had a situation where a grain would call another grain, which would call the first grain again. This would cause a deadlock.



## Future Work
### Add Authentication

### Add Storageprovider

### Follow Clean Architecture
Due to simplicity, DTOs, Domain Objects and Entities are mixed in the project. We should separate them.

### Use REST for public API
