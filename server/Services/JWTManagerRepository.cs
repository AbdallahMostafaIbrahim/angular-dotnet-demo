using TodoApi.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace TodoApi.Services
{
  public interface IJWTManagerRepository
  {
    string GenerateToken(string id);
  }
  public class JWTManagerRepository : IJWTManagerRepository
  {
    private readonly IConfiguration iconfiguration;
    public JWTManagerRepository(IConfiguration iconfiguration)
    {
      this.iconfiguration = iconfiguration;
    }

    public string GenerateToken(string id)
    {
      var tokenHandler = new JwtSecurityTokenHandler();
      var key = Encoding.ASCII.GetBytes(iconfiguration["Jwt:key"]);
      var tokenDescriptor = new SecurityTokenDescriptor
      {
        Subject = new ClaimsIdentity(new[] { new Claim("id", id) }),
        Expires = DateTime.UtcNow.AddDays(30),
        Issuer = iconfiguration["Jwt:Issuer"],
        Audience = iconfiguration["Jwt:Audience"],
        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
      };
      var token = tokenHandler.CreateToken(tokenDescriptor);
      return tokenHandler.WriteToken(token);
    }
  }

}