using Asp.Versioning;
using FafCarsApi.Models;
using FafCarsApi.Models.Dto;
using FafCarsApi.Models.Entities;
using FafCarsApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

  [Authorize(Roles = "Admin")]
  [HttpGet]
  [Route("{userId:Guid}")]
  public async Task<ActionResult<UserDto>> GetUser(Guid userId)
  {
    User? user = await _userService.FindUser(userId);
    if (user == null) return NotFound();
    return Ok(UserDto.FromUser(user));
  }

  [Authorize(Roles = "Admin")]
  [HttpGet]
  public async Task<ActionResult<PaginatedResultDto<UserDto>>> GetUsers([FromQuery] PaginationQuery pagination)
  {
    IQueryable<User> usersQuery = _userService.GetUsers();
    int count = await usersQuery.CountAsync();
    IList<UserDto> users = await usersQuery
      .OrderByDescending(u => u.CreatedAt)
      .Skip(pagination.Take * pagination.Page)
      .Take(pagination.Take)
      .Select(u => UserDto.FromUser(u))
      .ToListAsync();

    return Ok(new PaginatedResultDto<UserDto>
    {
      Items = users,
      TotalItems = count
    });
  }

  [HttpDelete]
  [Authorize(Roles = "Admin")]
  [Route("{userId:Guid}")]
  public async Task<OperationResultDto> DeleteUser(Guid userId)
  {
    _logger.LogInformation($"Deleted user {userId}");
    return await _userService.DeleteUser(userId);
  }

  [HttpPatch]
  [Authorize(Roles = "Admin")]
  [Route("{userId:Guid}")]
  public async Task<ActionResult> UpdateUser(Guid userId, [FromBody] UpdateUserDto updateDto)
  {
    User? user = await _userService.FindUser(userId);

    if (user == null)
      return NotFound();

    await _userService.UpdateUser(user, updateDto);
    return NoContent();
  }

  [HttpPost]
  [Authorize(Roles = "Admin")]
  public async Task<ActionResult> CreateUser([FromBody] CreateUserDto createDto)
  {
    if (await _userService.FindUserByUsername(createDto.Username) != null)
      return Conflict();

    await _userService.CreateUser(createDto);
    return Created();
  }
}
