using AutoMapper;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using SalesApi.src.Data;
using SalesApi.src.Models;
using SalesApi.src.Profiles;
using SalesApi.src.Services;
using Xunit;

namespace SalesApi.Tests;

public sealed class DebtQueryTests
{
    [Fact]
    public async Task GetByCustomerId_ReturnsOnlyActiveDebtsForRequestedCustomer()
    {
        await using var connection = new SqliteConnection("Data Source=:memory:");
        await connection.OpenAsync();
        await using var context = CreateContext(connection);
        await context.Database.EnsureCreatedAsync();
        await SeedAsync(context);

        var service = CreateService(context);
        var debts = service.GetByCustomerId(1);

        Assert.Single(debts);
        Assert.All(debts, debt => Assert.Equal(1, debt.CustomerId));
        Assert.All(debts, debt => Assert.Equal(1, debt.Active));
    }

    [Fact]
    public async Task HasDebitsPendinPayment_IgnoresPaidAndInactiveDebts()
    {
        await using var connection = new SqliteConnection("Data Source=:memory:");
        await connection.OpenAsync();
        await using var context = CreateContext(connection);
        await context.Database.EnsureCreatedAsync();
        await SeedAsync(context);

        var service = CreateService(context);
        Assert.True(service.HasDebitsPendinPayment(1));
        Assert.False(service.HasDebitsPendinPayment(2));
        Assert.False(service.HasDebitsPendinPayment(999));

        var pending = await context.Debts.SingleAsync(debt => debt.CustomerId == 1 && debt.Active == 1 && debt.PaidAt == null);
        pending.PaidAt = DateTime.UtcNow;
        await context.SaveChangesAsync();
        Assert.False(service.HasDebitsPendinPayment(1));
    }

    private static SalesContext CreateContext(SqliteConnection connection) =>
        new(new DbContextOptionsBuilder<SalesContext>().UseSqlite(connection).Options);

    private static DebtService CreateService(SalesContext context)
    {
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddAutoMapper(cfg => { }, typeof(DebtProfile).Assembly);
        using var provider = services.BuildServiceProvider();
        var mapper = provider.GetRequiredService<IMapper>();
        return new DebtService(NullLogger<DebtService>.Instance, context, mapper);
    }

    private static async Task SeedAsync(SalesContext context)
    {
        context.Customers.AddRange(
            new Customer { Id = 1, FullName = "Customer One", Cpf = "00000000000", BornDate = new DateOnly(1990, 1, 1), Active = 1 },
            new Customer { Id = 2, FullName = "Customer Two", Cpf = "11111111111", BornDate = new DateOnly(1991, 1, 1), Active = 1 });

        context.Debts.AddRange(
            new Debt { CustomerId = 1, value = 100, CreatedAt = DateTime.UtcNow, Active = 1 },
            new Debt { CustomerId = 1, value = 150, CreatedAt = DateTime.UtcNow, Active = 0 },
            new Debt { CustomerId = 2, value = 200, CreatedAt = DateTime.UtcNow, Active = 1, PaidAt = DateTime.UtcNow },
            new Debt { CustomerId = 2, value = 250, CreatedAt = DateTime.UtcNow, Active = 0 });
        await context.SaveChangesAsync();
    }
}
