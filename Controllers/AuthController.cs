using Asp.Versioning;
using FafCarsApi.Models.Dto;
using FafCarsApi.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using FafCarsApi.Services;
using Microsoft.AspNetCore.Authorization;

namespace FafCarsApi.Controllers;

[ApiController]
[ApiVersion(1)]
[Route("api/v{v:apiVersion}/auth")]
public class AuthController : Controller
{
  private readonly AuthService _authService;
  private readonly UserService _userService;

  public AuthController(
    AuthService authService,
    UserService userService
  )
  {
    _authService = authService;
    _userService = userService;
  }

  [HttpPost]
  [Route("login")]
  [AllowAnonymous]
  public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
  {
    User? user = await _userService.FindUserByUsername(loginDto.Username);

    if (user == null)
      return NotFound();

    if (!_authService.ValidatePassword(user, loginDto.Password))
      return Unauthorized();

    return Ok(
      new JwtResponse
      {
        Token = _authService.GenerateJwt(user)
      }
    );
  }

  [HttpPost]
  [Route("register")]
  [AllowAnonymous]
  public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
  {
    User? user = await _userService.FindUserByUsername(registerDto.Username);

    if (user != null)
      return Conflict();

    User newUser = await _userService.CreateUser(registerDto);

    return Ok(
      new JwtResponse
      {
        Token = _authService.GenerateJwt(newUser)
      }
    );
  }
}
