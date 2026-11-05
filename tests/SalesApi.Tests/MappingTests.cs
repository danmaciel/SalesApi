using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using SalesApi.src.Profiles;
using Xunit;

namespace SalesApi.Tests;

public sealed class MappingTests
{
    [Fact]
    public void CustomerProfile_MapsCreateDtoToCustomer()
    {
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddAutoMapper(cfg => { }, typeof(CustomerProfile).Assembly);
        using var provider = services.BuildServiceProvider();
        var mapper = provider.GetRequiredService<IMapper>();

        var input = new CreateCustomerDto
        {
            FullName = "Test User",
            Cpf = "00000000000",
            BornDate = new DateOnly(1990, 1, 1),
            Email = "test@example.invalid"
        };

        var customer = mapper.Map<Customer>(input);
        Assert.Equal(input.FullName, customer.FullName);
        Assert.Equal(input.Cpf, customer.Cpf);
        Assert.Equal(input.BornDate, customer.BornDate);
        Assert.Equal(input.Email, customer.Email);
    }
}
