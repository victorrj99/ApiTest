using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ApiOwn.Models;
using Microsoft.IdentityModel.Tokens;

namespace ApiOwn.Services;

public class TokenService
{
    public string GenerateToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(Configuration.JwtKey);
        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity([
                new Claim(ClaimTypes.Name, "João Victor"), // User.Identity.Name
                new Claim(ClaimTypes.Role, "Admin"), // User.IsInRole
                new Claim("fruta", "banana")
            ]),
            Expires = DateTime.UtcNow.AddHours(10),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature) 
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}