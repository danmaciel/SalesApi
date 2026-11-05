using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace SalesApi.Tests;

public sealed class HttpIntegrationTests : IAsyncLifetime
{
    private const string TestSigningKey = "test-only-http-integration-signing-key-1234567890";
    private readonly string _dbPath = Path.Combine(Path.GetTempPath(), $"salesapi-test-{Guid.NewGuid():N}.db");
    private WebApplicationFactory<Program> _factory = null!;
    private HttpClient _client = null!;

    public Task InitializeAsync()
    {
        _factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
        {
            builder.UseEnvironment("Testing");
            builder.ConfigureAppConfiguration((_, configuration) =>
                configuration.AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["SymmetricSecurityKey:value"] = TestSigningKey,
                    ["ConnectionStrings:WebApiDatabase"] = $"Data Source={_dbPath}"
                }));
        });
        _client = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
        return Task.CompletedTask;
    }

    [Fact]
    public async Task CustomerEndpoint_RejectsAnonymousRequest()
    {
        using var response = await _client.GetAsync("/Customer");
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task RegistrationAndLogin_AuthorizeCustomerEndpoint()
    {
        var username = $"user-{Guid.NewGuid():N}";
        var password = "Valid1!Password123";

        using var registration = await _client.PostAsJsonAsync("/User", new
        {
            userName = username,
            password,
            rePassword = password
        });
        Assert.Equal(HttpStatusCode.OK, registration.StatusCode);

        using var login = await _client.PostAsJsonAsync("/User/authenticate", new
        {
            userName = username,
            password
        });
        Assert.Equal(HttpStatusCode.OK, login.StatusCode);
        var token = await login.Content.ReadFromJsonAsync<string>();
        Assert.False(string.IsNullOrWhiteSpace(token));

        using var request = new HttpRequestMessage(HttpMethod.Get, "/Customer");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        using var response = await _client.SendAsync(request);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Registration_RejectsInvalidPayload()
    {
        using var response = await _client.PostAsJsonAsync("/User", new
        {
            userName = "",
            password = "",
            rePassword = ""
        });
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    public async Task DisposeAsync()
    {
        _client?.Dispose();
        if (_factory is not null) await _factory.DisposeAsync();
        if (File.Exists(_dbPath)) File.Delete(_dbPath);
    }
}
