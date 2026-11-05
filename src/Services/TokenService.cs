using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace SalesApi.src.Services;

public class TokenService{

    private IConfiguration _configuration;

    public TokenService(IConfiguration configuration){
        _configuration = configuration;
    }
    
    public string GenerateToken(User user) {
            
         var claims = new Claim[]{
            new Claim("username", user.UserName!),
            new Claim("id", user.Id),
            new Claim("loginTimestamp", DateTime.UtcNow.ToString()),
         };

        var secret = _configuration["SymmetricSecurityKey:value"] ?? throw new InvalidOperationException("JWT signing key is not configured.");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        
        var token = new JwtSecurityToken(
            expires: DateTime.Now.AddDays(30),
            claims: claims,
            signingCredentials: signingCredentials
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}