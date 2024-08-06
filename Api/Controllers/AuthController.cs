using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Api.Dtos.Request;
using Api.Dtos.Response;
using Api.Models;
using Api.Services;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[ApiVersion(1)]
[AllowAnonymous]
[Route("Api/v{v:apiVersion}/[controller]")]
public class AuthController(
  AuthService authService,
  UserService userService
) : Controller {
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

    authService.CacheRefreshToken(user.Id, refreshToken);

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

    authService.CacheRefreshToken(newUser.Id, refreshToken);

    return response;
  }

  [HttpPost("Refresh")]
  public async Task<ActionResult<JwtResponseDto>> Refresh([FromBody] JwtResponseDto responseDto) {
    ClaimsPrincipal? principal = authService.ValidateToken(responseDto.Token);

    if (principal == null) {
      return BadRequest("invalid token");
    }

    string userIdStr = principal.FindFirst(JwtRegisteredClaimNames.Sub)!.Value;
    Guid userId = Guid.Parse(userIdStr);
    User? user = await userService.FindUser(userId);

    if (user == null) {
      return NotFound("user not found");
    }

    string? refreshToken = await authService.GetCachedRefreshToken(userId);

    if (refreshToken == null || refreshToken != responseDto.RefreshToken) {
      return Unauthorized();
    }

    string newAccessToken = authService.GenerateAccessToken(user);

    return new JwtResponseDto {
      Token = newAccessToken,
      RefreshToken = responseDto.RefreshToken
    };
  }
}
