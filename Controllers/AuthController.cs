using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Asp.Versioning;
using FafCarsApi.Models.Dto;
using FafCarsApi.Models.Entities;
using FafCarsApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FafCarsApi.Controllers;

[ApiController]
[ApiVersion(1)]
[AllowAnonymous]
[Route("api/v{v:apiVersion}/[controller]")]
public class AuthController(
  AuthService authService,
  UserService userService
) : Controller {
  private readonly Dictionary<Guid, string> _refreshTokens = new();

  [HttpPost("login")]
  public async Task<ActionResult<JwtResponseDto>> Login([FromBody] LoginDto loginDto) {
    User? user = await userService.FindUserByUsername(loginDto.Username);

    if (user == null)
      return NotFound();

    if (!authService.ValidatePassword(user, loginDto.Password))
      return Unauthorized();

    List<Claim> claims = authService.GetUserClaims(user);
    string token = authService.GenerateAccessToken(claims);
    string refreshToken = authService.GenerateRefreshToken();
    var response = new JwtResponseDto {
      Token = token,
      RefreshToken = refreshToken
    };

    _refreshTokens.Add(user.Id, refreshToken);

    return Ok(response);
  }

  [HttpPost("register")]
  public async Task<ActionResult<JwtResponseDto>> Register([FromBody] RegisterDto registerDto) {
    User? user = await userService.FindUserByUsername(registerDto.Username);

    if (user != null)
      return Conflict();

    User newUser = await userService.RegisterUser(registerDto);
    List<Claim> claims = authService.GetUserClaims(newUser);
    string token = authService.GenerateAccessToken(claims);
    string refreshToken = authService.GenerateRefreshToken();
    var response = new JwtResponseDto {
      Token = token,
      RefreshToken = refreshToken
    };

    _refreshTokens.Add(newUser.Id, refreshToken);

    return Ok(response);
  }

  [HttpPost("refresh")]
  public async Task<ActionResult<JwtResponseDto>> Refresh([FromBody] JwtResponseDto responseDto) {
    ClaimsPrincipal? principal = authService.ValidateToken(responseDto.Token);

    if (principal == null)
      return BadRequest("invalid token");

    Guid userId = Guid.Parse(principal.FindFirst(JwtRegisteredClaimNames.Sub)!.Value);
    User? user = await userService.FindUser(userId);

    if (user == null)
      return NotFound("user not found");

    if (_refreshTokens.ContainsKey(userId))
      return BadRequest("no token in tokens list");

    List<Claim> claims = principal.Claims
      .Where(c => c.Type != JwtRegisteredClaimNames.Aud &&
                  c.Type != JwtRegisteredClaimNames.Iss &&
                  c.Type != JwtRegisteredClaimNames.Nbf &&
                  c.Type != JwtRegisteredClaimNames.Iat &&
                  c.Type != JwtRegisteredClaimNames.Exp)
      .ToList();

    string newToken = authService.GenerateAccessToken(claims);

    return Ok(new JwtResponseDto {
      Token = newToken,
      RefreshToken = responseDto.RefreshToken
    });
  }
}
