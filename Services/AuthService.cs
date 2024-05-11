using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FafCarsApi.Models;
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

    var claims = new List<Claim>
    {
      new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
      new Claim(JwtRegisteredClaimNames.Name, user.Username),
      new Claim(JwtRegisteredClaimNames.Email, user.Email),
      new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };   
    
    foreach (UserRole role in user.Roles)
      claims.Add(new Claim(ClaimTypes.Role, role.ToString()));
    
    var tokenDescriptor = new SecurityTokenDescriptor
    {
      Subject = new ClaimsIdentity(claims),
      Expires = DateTime.UtcNow.AddMinutes(int.Parse(_config["JWT:TTL"]!)),
      Issuer = issuer,
      Audience = audience,
      SigningCredentials = new SigningCredentials
      (
        new SymmetricSecurityKey(key),
        SecurityAlgorithms.HmacSha512Signature
      )
    };
    var tokenHandler = new JwtSecurityTokenHandler();
    SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
    return tokenHandler.WriteToken(token);
  }
}
