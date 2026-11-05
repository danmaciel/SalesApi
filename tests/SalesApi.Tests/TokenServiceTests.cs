using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SalesApi.src.Services;
using Xunit;

namespace SalesApi.Tests;

public sealed class TokenServiceTests
{
    private const string SigningKey = "test-only-signing-key-not-for-use-in-production-12345";

    [Fact]
    public void GenerateToken_SignsExpectedClaims()
    {
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["SymmetricSecurityKey:value"] = SigningKey
            }).Build();
        var service = new TokenService(config);
        var user = new User { Id = "user-123", UserName = "test-user" };

        var jwt = service.GenerateToken(user);
        var handler = new JwtSecurityTokenHandler();
        var principal = handler.ValidateToken(jwt, new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SigningKey)),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        }, out var validatedToken);

        Assert.IsType<JwtSecurityToken>(validatedToken);
        Assert.Equal("test-user", principal.FindFirst("username")?.Value);
        Assert.Equal("user-123", principal.FindFirst("id")?.Value);
        Assert.NotNull(principal.FindFirst("loginTimestamp"));
    }

    [Fact]
    public void GenerateToken_ThrowsWhenSigningKeyMissing()
    {
        var service = new TokenService(new ConfigurationBuilder().Build());
        Assert.Throws<InvalidOperationException>(() =>
            service.GenerateToken(new User { Id = "user-1", UserName = "test" }));
    }
}
