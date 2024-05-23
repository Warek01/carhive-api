using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using FafCarsApi.Models.Entities;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames;

namespace FafCarsApi.Services;

public class AuthService(IConfiguration config, ILogger<AuthService> logger) {
  public static TokenValidationParameters GetTokenValidationParameters(IConfiguration config) {
    return new TokenValidationParameters {
      NameClaimType = "sub",
      RoleClaimType = "role",
      ValidIssuer = config["Jwt:Issuer"],
      ValidAudience = config["Jwt:Audience"],
      IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!)),
      ValidateIssuer = true,
      ValidateAudience = true,
      ValidateLifetime = true,
      ValidateIssuerSigningKey = true,
      ClockSkew = TimeSpan.Zero,
      RequireAudience = true,
      RequireExpirationTime = true,
      RequireSignedTokens = true,
      LogValidationExceptions = true,
      LogTokenId = true
    };
  }

  public bool ValidatePassword(User user, string password) {
    return BCrypt.Net.BCrypt.EnhancedVerify(password, user.Password);
  }

  public ClaimsPrincipal? ValidateToken(string token) {
    var tokenHandler = new JwtSecurityTokenHandler();
    var parameters = GetTokenValidationParameters(config);
    parameters.ValidateLifetime = false;

    try {
      return tokenHandler.ValidateToken(token, parameters, out var _);
    }
    catch (Exception ex) {
      logger.LogInformation(ex.ToString());
      return null;
    }
  }

  public string GenerateRefreshToken() {
    var randomNumber = new byte[32];
    using var rng = RandomNumberGenerator.Create();
    rng.GetBytes(randomNumber);
    return Convert.ToBase64String(randomNumber);
  }

  public List<Claim> GetUserClaims(User user) {
    var claims = new List<Claim> {
      new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
      new(JwtRegisteredClaimNames.Name, user.Username),
      new(JwtRegisteredClaimNames.Email, user.Email),
      new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

    if (user.PhoneNumber != null)
      claims.Add(new Claim("phone", user.PhoneNumber));

    foreach (var role in user.Roles)
      claims.Add(new Claim("role", role.ToString()));

    return claims;
  }

  public string GenerateAccessToken(List<Claim> claims) {
    var issuer = config["Jwt:Issuer"]!;
    var audience = config["Jwt:Audience"]!;
    var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(config["Jwt:Key"]!));
    var tokenTtl = int.Parse(config["JWT:TTL"]!);

    var tokenDescriptor = new SecurityTokenDescriptor {
      Subject = new ClaimsIdentity(claims),
      Expires = DateTime.UtcNow.AddMinutes(tokenTtl),
      Issuer = issuer,
      Audience = audience,
      SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature)
    };
    var tokenHandler = new JwtSecurityTokenHandler();
    var token = tokenHandler.CreateToken(tokenDescriptor);
    return tokenHandler.WriteToken(token);
  }

  public static void SetupAuthorization(WebApplication app) {
    app.UseAuthentication();
    app.UseRouting();
    JsonWebTokenHandler.DefaultInboundClaimTypeMap.Clear();
    JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
    JsonWebTokenHandler.DefaultMapInboundClaims = false;
    JwtSecurityTokenHandler.DefaultMapInboundClaims = false;
    app.UseAuthorization();
  }
}