# SalesApi

ASP.NET Core 10 Web API project with SQLite and JWT authentication.

## Local security configuration

This project intentionally does not commit the JWT signing key. The application requires a signing key of at least 32 UTF-8 bytes at startup.

For local development, use the built-in .NET user-secrets feature:

```bash
dotnet user-secrets set "SymmetricSecurityKey:value" "<your-random-secret-of-at-least-32-bytes>"
dotnet run
```

Generate a unique, unpredictable secret and do not commit it to source control. In deployment environments, provide the value through a secret manager or the environment variable `SymmetricSecurityKey__value`.

The SQLite connection string is configured in `appsettings.json`. Ensure that the referenced local database directory exists before running migrations.

## Security note

Older commits contained a hardcoded JWT signing key. Treat that value as compromised: do not reuse it, and rotate it wherever it has been deployed. Removing it from the current branch does not erase it from Git history. Previously issued tokens signed with the old key should be invalidated through key rotation.

## Build

```bash
dotnet restore
dotnet build
```

The project targets .NET 10 LTS. Run `dotnet restore`, `dotnet build`, and review NuGet package audit output before deployment.
