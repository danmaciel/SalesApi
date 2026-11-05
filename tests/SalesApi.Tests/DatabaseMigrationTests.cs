using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using SalesApi.src.Data;
using Xunit;

namespace SalesApi.Tests;

public sealed class DatabaseMigrationTests
{
    [Fact]
    public async Task Migrate_CreatesDatabaseSchema()
    {
        await using var connection = new SqliteConnection("Data Source=:memory:");
        await connection.OpenAsync();
        var options = new DbContextOptionsBuilder<SalesContext>()
            .UseSqlite(connection)
            .Options;

        await using var context = new SalesContext(options);
        await context.Database.MigrateAsync();

        var tables = await context.Database.SqlQueryRaw<string>(
            "SELECT name AS Value FROM sqlite_master WHERE type='table' AND name='Customers'")
            .ToListAsync();

        Assert.Contains("Customers", tables);
    }
}
