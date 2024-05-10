using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FafCarsApi.Models.Entities;
using Microsoft.IdentityModel.Tokens;

namespace FafCarsApi.Services;

public class AuthService
{
  private readonly IConfiguration _config;

  public AuthService(IConfiguration config)
  {
    _config = config;
  }

  public bool ValidatePassword(User user, string password)
  {
    return BCrypt.Net.BCrypt.EnhancedVerify(password, user.Password);
  }

  public string GenerateJwt(User user)
  {
    string issuer = _config["Jwt:Issuer"]!;
    string audience = _config["Jwt:Audience"]!;
    var key = Encoding.ASCII.GetBytes(_config["Jwt:Key"]!);
    var tokenDescriptor = new SecurityTokenDescriptor
    {
      Subject = new ClaimsIdentity(new[]
      {
        new Claim("Id", Guid.NewGuid().ToString()),
        new Claim(JwtRegisteredClaimNames.Sub, user.Username),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
      }),
      Expires = DateTime.UtcNow.AddHours(int.Parse(_config["JWT:TTL"]!)),
      Issuer = issuer,
      Audience = audience,
      SigningCredentials = new SigningCredentials
      (new SymmetricSecurityKey(key),
        SecurityAlgorithms.HmacSha512Signature)
    };
    var tokenHandler = new JwtSecurityTokenHandler();
    SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
    return tokenHandler.WriteToken(token);
  }
}
