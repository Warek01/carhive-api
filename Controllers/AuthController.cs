using System.IdentityModel.Tokens.Jwt;
using Asp.Versioning;
using FafCarsApi.Models.Dto;
using FafCarsApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FafCarsApi.Controllers;

[ApiController]
[ApiVersion(1)]
[AllowAnonymous]
[Route("api/v{v:apiVersion}/auth")]
public class AuthController(
  AuthService authService,
  UserService userService
) : Controller {
  private readonly Dictionary<Guid, string> _refreshTokens = new();

  [HttpPost("login")]
  public async Task<ActionResult<JwtResponseDto>> Login([FromBody] LoginDto loginDto) {
    var user = await userService.FindUserByUsername(loginDto.Username);

    if (user == null)
      return NotFound();

    if (!authService.ValidatePassword(user, loginDto.Password))
      return Unauthorized();

    var claims = authService.GetUserClaims(user);
    var token = authService.GenerateAccessToken(claims);
    var refreshToken = authService.GenerateRefreshToken();
    var response = new JwtResponseDto {
      Token = token,
      RefreshToken = refreshToken
    };

    _refreshTokens.Add(user.Id, refreshToken);

    return Ok(response);
  }

  [HttpPost("register")]
  public async Task<ActionResult<JwtResponseDto>> Register([FromBody] RegisterDto registerDto) {
    var user = await userService.FindUserByUsername(registerDto.Username);

    if (user != null)
      return Conflict();

    var newUser = await userService.RegisterUser(registerDto);

    var claims = authService.GetUserClaims(newUser);
    var token = authService.GenerateAccessToken(claims);
    var refreshToken = authService.GenerateRefreshToken();
    var response = new JwtResponseDto {
      Token = token,
      RefreshToken = refreshToken
    };

    _refreshTokens.Add(newUser.Id, refreshToken);

    return Ok(response);
  }

  [HttpPost("refresh")]
  public async Task<ActionResult<JwtResponseDto>> Refresh([FromBody] JwtResponseDto responseDto) {
    var principal = authService.ValidateToken(responseDto.Token);

    if (principal == null)
      return BadRequest("invalid token");

    var userId = Guid.Parse(principal.FindFirst(JwtRegisteredClaimNames.Sub)!.Value);
    var user = await userService.FindUser(userId);

    if (user == null)
      return NotFound("user not found");

    if (_refreshTokens.ContainsKey(userId))
      return BadRequest("no token in tokens list");

    var claims = principal.Claims
      .Where(c => c.Type != JwtRegisteredClaimNames.Aud &&
                  c.Type != JwtRegisteredClaimNames.Iss &&
                  c.Type != JwtRegisteredClaimNames.Nbf &&
                  c.Type != JwtRegisteredClaimNames.Iat &&
                  c.Type != JwtRegisteredClaimNames.Exp)
      .ToList();

    var newToken = authService.GenerateAccessToken(claims);

    return Ok(new JwtResponseDto {
      Token = newToken,
      RefreshToken = responseDto.RefreshToken
    });
  }
}