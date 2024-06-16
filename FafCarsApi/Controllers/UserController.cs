using Asp.Versioning;
using AutoMapper;
using FafCarsApi.Dtos;
using FafCarsApi.Helpers;
using FafCarsApi.Models;
using FafCarsApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FafCarsApi.Controllers;

[ApiController]
[ApiVersion(1)]
[Route("Api/v{v:apiVersion}/[controller]")]
public class UserController(
  UserService userService,
  IMapper mapper
) : Controller {
  [HttpGet]
  [Route("{userId:Guid}")]
  [Authorize]
  public async Task<ActionResult<UserDto>> GetUser(Guid userId) {
    User? queriedUser = await userService.FindUser(userId);

    if (queriedUser == null)
      return NotFound();

    UserDto result = mapper.Map<UserDto>(queriedUser);

    bool isAdmin = User.HasClaim(c => c is { Type: "role", Value: "Admin" });

    if (!isAdmin) {
      result.Roles = null;
      result.Email = null;
    }

    return result;
  }

  [Authorize(Roles = "Admin")]
  [HttpGet]
  public async Task<ActionResult<PaginatedResultDto<UserDto>>> GetUsers([FromQuery] PaginationQuery pagination) {
    var usersQuery = userService.GetUsers();
    var count = await usersQuery.CountAsync();
    List<UserDto> users = await usersQuery
      .OrderByDescending(u => u.CreatedAt)
      .Skip(pagination.Take * pagination.Page)
      .Take(pagination.Take)
      .Select(u => mapper.Map<UserDto>(u))
      .ToListAsync();

    return new PaginatedResultDto<UserDto> {
      Items = users,
      TotalItems = count
    };
  }

  [HttpDelete]
  [Authorize(Roles = "Admin")]
  [Route("{userId:Guid}")]
  public async Task<ActionResult> DeleteUser(Guid userId) {
    User? user = await userService.DeleteUser(userId);
    return user == null ? NotFound() : Ok();
  }

  [HttpPatch]
  [Authorize(Roles = "Admin")]
  [Route("{userId:Guid}")]
  public async Task<ActionResult> UpdateUser(Guid userId, [FromBody] UpdateUserDto updateDto) {
    var user = await userService.FindUser(userId);

    if (user == null)
      return NotFound();

    await userService.UpdateUser(user, updateDto);
    return NoContent();
  }

  [HttpPost]
  [Authorize(Roles = "Admin")]
  public async Task<ActionResult> CreateUser([FromBody] CreateUserDto createDto) {
    User? existingUser = await userService.FindUserByUsername(createDto.Username);

    if (existingUser != null)
      return Conflict();

    await userService.CreateUser(createDto);
    return Created();
  }
}
