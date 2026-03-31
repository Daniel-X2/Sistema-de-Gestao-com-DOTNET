using System.IdentityModel.Tokens.Jwt;
using System.Text;
using auth.Models;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Dto;

namespace auth.Services;

public class TokenService
{
    
    public string Generate(Users user,bool isadmin)
    {
        var handler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_KEY"));
       var credetial= new SigningCredentials(new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha256Signature);

       var tokenDescriptor = new SecurityTokenDescriptor
       {
           Subject = GenerateClaims(user,isadmin ),
           SigningCredentials = credetial,
           Expires = DateTime.UtcNow.AddHours(2),
           Issuer = Environment.GetEnvironmentVariable("JWT_ISSUER"),
           Audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE")

       };
       var token = handler.CreateToken(tokenDescriptor);

        var strToken = handler.WriteToken(token);
        return strToken;
    }

    private static ClaimsIdentity GenerateClaims(Users users,bool isadmin)
    {
        var ci = new ClaimsIdentity();
        ci.AddClaim(new Claim(ClaimTypes.Name,users.cpf));
        if (isadmin)
        {
            ci.AddClaim(new Claim(ClaimTypes.Role,"Admin"));
            return ci;
        }
        ci.AddClaim(new Claim(ClaimTypes.Role,"User"));
        return ci;
    }
}