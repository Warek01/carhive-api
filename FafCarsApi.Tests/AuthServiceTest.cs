using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using FafCarsApi.Enums;
using FafCarsApi.Models;
using FafCarsApi.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace FafCarsApi.Tests;

public partial class AuthServiceTest {
  private readonly AuthService _authService;
  private readonly Mock<IConfiguration> _configMock;
  private readonly Mock<ILogger<AuthService>> _loggerMock;

  public AuthServiceTest() {
    _configMock = new Mock<IConfiguration>();
    _loggerMock = new Mock<ILogger<AuthService>>();
    _authService = new AuthService(_configMock.Object, _loggerMock.Object);
  }

  [Theory]
  [InlineData("test", "test")]
  [InlineData("correct", "correct")]
  [InlineData("abcde123412341234", "abcde123412341234")]
  public void VerifyPassword_Returns_TrueForCorrectPasswords(string actualPassword, string password) {
    string hashed = BCrypt.Net.BCrypt.EnhancedHashPassword(actualPassword, 13);
    var user = new User {
      Password = hashed
    };

    bool result = _authService.ValidatePassword(user, password);

    result.Should().BeTrue();
  }

  [Theory]
  [InlineData("test", "test1")]
  [InlineData("wrong", "correct")]
  [InlineData("abcde123412341234___", "abcde123412341234")]
  public void VerifyPassword_Returns_FalseForWrongPasswords(string actualPassword, string password) {
    string hashed = BCrypt.Net.BCrypt.EnhancedHashPassword(actualPassword, 13);
    var user = new User {
      Password = hashed
    };

    bool result = _authService.ValidatePassword(user, password);

    result.Should().BeFalse();
  }

  [Fact]
  public void GenerateRefreshToken_Returns_NotNull() {
    string? token = _authService.GenerateRefreshToken();

    token.Should().NotBeNull();
    token.Should().NotBeEmpty();
  }

  [Fact]
  public void GetUserClaims_Returns_CorrectClaims() {
    var user = new User {
      Id = Guid.NewGuid(),
      Roles = [UserRole.Admin, UserRole.ListingCreator],
      Username = "warek"
    };
    List<string> requiredClaims = [
      JwtRegisteredClaimNames.Jti, JwtRegisteredClaimNames.Sub, "role"
    ];
    List<string> forbiddenClaims = [
      "password", "Password"
    ];

    IEnumerable<Claim> claims = _authService.GetUserClaims(user);

    claims.Should().Contain(c => requiredClaims.Contains(c.Type));
    claims.Should().NotContain(c => forbiddenClaims.Contains(c.Type));
  }
}