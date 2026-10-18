using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using SummerStore.Core.Interfaces;
using SummerStore.Core.Models.Entities;

namespace SummerStore.Core.Services;

public class JwtService(IOptions<AuthSettings> options) : IJwtService
{
    public string GenerateToken(User user)
    {
        var claims = new List<Claim>{
            new("userName", user.UserName ?? ""),
            new("id", user.Id.ToString()),
        };

        var jwtToken = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.Add(options.Value.Expires),
            signingCredentials:
            new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Value.SecretKey)),
                SecurityAlgorithms.HmacSha256));

        return new JwtSecurityTokenHandler().WriteToken(jwtToken);
    }
}