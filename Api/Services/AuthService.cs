using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Api.Models;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using JsonClaimValueTypes = System.IdentityModel.Tokens.Jwt.JsonClaimValueTypes;
using JwtRegisteredClaimNames = System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames;

namespace Api.Services;

public class AuthService(
  IConfiguration config,
  ILogger<AuthService> logger,
  CacheService cache
) {
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
      return tokenHandler.ValidateToken(token, parameters, out _);
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

  public string GenerateAccessToken(User user) {
    var issuer = config["Jwt:Issuer"]!;
    var audience = config["Jwt:Audience"]!;
    var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(config["Jwt:Key"]!));
    var tokenTtl = int.Parse(config["JWT:Ttl"]!);
    var identity = new ClaimsIdentity([
      new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
      new Claim(
        ClaimTypes.Role,
        JsonSerializer.Serialize(user.Roles.Select(r => r.ToString())),
        JsonClaimValueTypes.JsonArray
      )
    ]);

    var tokenDescriptor = new SecurityTokenDescriptor {
      Subject = identity,
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

  public void CacheRefreshToken(Guid userId, string refreshToken) {
    var refreshTtl = double.Parse(config["JWT:RefreshTtl"]!);

    cache.Db.StringSetAsync(
      $"{CacheService.Keys.RefreshTokens}:{userId.ToString()}",
      refreshToken,
      TimeSpan.FromMinutes(refreshTtl),
      flags: CommandFlags.FireAndForget
    );
  }

  public async Task<string?> GetCachedRefreshToken(Guid userId) {
    string? refreshToken = await cache.Db.StringGetAsync($"{CacheService.Keys.RefreshTokens}:{userId.ToString()}");
    return refreshToken;
  }
}
