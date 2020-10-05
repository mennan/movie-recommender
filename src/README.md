# Movie Recommender API

## Requirements

- .NET Core SDK 3.1
- PostgreSql
- Auth0 Account
- TMDb Account

## Installation

1. Edit the contents of the `src/MovieRecommender.Api/appsettings.json` file according to your own settings.
2. Please type below command to your terminal for create database schema.

```bash
yarn db-update
```

or

```bash
npm run db-update
```

3. Navigate to `src/MovieRecommender.Api` folder and type `dotnet run` command.
4. Application will be serving on `https://localhost:5001`

## Documentation

Go to `https://localhost:5001/help` for API documentation.
