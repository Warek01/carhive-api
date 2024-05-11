using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Asp.Versioning;
using FafCarsApi.Models;
using FafCarsApi.Models.Dto;
using FafCarsApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FafCarsApi.Controllers;

[ApiController]
[ApiVersion(1)]
[AllowAnonymous]
[Route("api/v{v:apiVersion}/user")]
public class UserController : Controller
{
  private readonly UserService _userService;
  private readonly ILogger<UserController> _logger;

  public UserController(
    UserService userService,
    ILogger<UserController>  logger
    )
  {
    _userService = userService;
    _logger = logger;
  }

  [HttpGet]
  public async Task<ICollection<UserDto>> GetUsers([FromQuery] PaginationQuery pagination)
  {
    return await _userService.GetUsers(pagination);
  }

  [HttpDelete]
  [Route("{userId:guid}")]
  public async Task<OperationResultDto> DeleteUser(Guid userId)
  {
    return await _userService.DeleteUser(userId);
  }
}
