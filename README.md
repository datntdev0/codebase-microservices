# codebase-microservices
This repository contains the boilerplate for a microservices-based system. It includes multiple backend services built with ASP.NET Core and an Angular frontend for the administration page.

## Database migration with ASP.NET Boilerplate Using Entity Framework

### Delete All Migrations - WARNING: this step clean up your existing database into empty database

1. Open the Package Manager Console: In Visual Studio, go to Tools > NuGet Package Manager > Package Manager Console.
2. Clean Up the current database: This will revert your database to the state it was in before any migrations were applied.

    ```powershell
    Update-Database -Migration:0
    ```
3. Remove Migration Snapshot: Delete the `Migrations` folder within your .EntityFramework project. You can do this directly from the Solution Explorer.

### Re-generate Initial Migration or Create new Database Migration

1. Generate a New Initial Migration: In the Package Manager Console, run the following command to generate a new initial migration: This command will create a new migration file in the Migrations folder.

    ```powershell
    Add-Migration InitialCreate
    ```

2. Apply the New Initial Migration: Run the following command to apply the new initial migration to the database:

    ```powershell
    Update-Database
    ```
