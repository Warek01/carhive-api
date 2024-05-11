using Asp.Versioning;
using FafCarsApi.Models;
using FafCarsApi.Models.Dto;
using FafCarsApi.Models.Entities;
using FafCarsApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FafCarsApi.Controllers;

[ApiController]
[ApiVersion(1)]
[Route("api/v{v:apiVersion}/user")]
public class UserController : Controller
{
  private readonly UserService _userService;
  private readonly ILogger<UserController> _logger;

  public UserController(
    UserService userService,
    ILogger<UserController> logger
  )
  {
    _userService = userService;
    _logger = logger;
  }

  [Authorize(Roles = "Admin,User")]
  [HttpGet]
  [Route("{userId:guid}")]
  public async Task<ActionResult<UserDto>> GetUser(Guid userId)
  {
    User? user = await _userService.FindUser(userId);
    if (user == null) return NotFound();
    return Ok(UserDto.CreateFromUser(user));
  }

  [Authorize(Roles = "Admin")]
  [HttpGet]
  public async Task<ICollection<UserDto>> GetUsers([FromQuery] PaginationQuery pagination)
  {
    return await _userService.GetUsers(pagination);
  }

  [HttpDelete]
  [Authorize(Roles = "Admin,SelfDelete")]
  [Route("{userId:guid}")]
  public async Task<OperationResultDto> DeleteUser(Guid userId)
  {
    _logger.LogInformation($"Deleted user {userId}");
    return await _userService.DeleteUser(userId);
  }
}
