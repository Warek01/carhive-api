using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Asp.Versioning;
using FafCarsApi.Dtos;
using FafCarsApi.Dtos.Request;
using FafCarsApi.Dtos.Response;
using FafCarsApi.Models;
using FafCarsApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FafCarsApi.Controllers;

[ApiController]
[ApiVersion(1)]
[AllowAnonymous]
[Route("Api/v{v:apiVersion}/[controller]")]
public class AuthController(
  AuthService authService,
  UserService userService
) : Controller {
  private readonly Dictionary<Guid, string> _refreshTokens = [];

  [HttpPost("Login")]
  public async Task<ActionResult<JwtResponseDto>> Login([FromBody] LoginDto loginDto) {
    User? user = await userService.FindUserByUsername(loginDto.Username);

    if (user == null) {
      return NotFound();
    }

    if (!authService.ValidatePassword(user, loginDto.Password)) {
      return Unauthorized();
    }

    string token = authService.GenerateAccessToken(user);
    string refreshToken = authService.GenerateRefreshToken();
    var response = new JwtResponseDto {
      Token = token,
      RefreshToken = refreshToken
    };

    _refreshTokens.Add(user.Id, refreshToken);

    return response;
  }

  [HttpPost("Register")]
  public async Task<ActionResult<JwtResponseDto>> Register([FromBody] RegisterDto registerDto) {
    User? user = await userService.FindUserByUsername(registerDto.Username);

    if (user != null) {
      return Conflict();
    }

    User newUser = await userService.RegisterUser(registerDto);
    string token = authService.GenerateAccessToken(user!);
    string refreshToken = authService.GenerateRefreshToken();
    var response = new JwtResponseDto {
      Token = token,
      RefreshToken = refreshToken
    };

    _refreshTokens.Add(newUser.Id, refreshToken);

    return response;
  }

  [HttpPost("Refresh")]
  public async Task<ActionResult<JwtResponseDto>> Refresh([FromBody] JwtResponseDto responseDto) {
    ClaimsPrincipal? principal = authService.ValidateToken(responseDto.Token);

    if (principal == null) {
      return BadRequest("invalid token");
    }

    Guid userId = Guid.Parse(principal.FindFirst(JwtRegisteredClaimNames.Sub)!.Value);
    User? user = await userService.FindUser(userId);

    if (user == null) {
      return NotFound("user not found");
    }

    if (_refreshTokens.ContainsKey(userId)) {
      return BadRequest("no token in tokens list");
    }

    string newToken = authService.GenerateAccessToken(user);

    return new JwtResponseDto {
      Token = newToken,
      RefreshToken = responseDto.RefreshToken
    };
  }
}
