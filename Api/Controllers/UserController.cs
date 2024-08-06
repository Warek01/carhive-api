using Api.Dtos.Request;
using Api.Dtos.Response;
using Api.Helpers;
using Api.Models;
using Api.Queries;
using Api.Services;
using Asp.Versioning;
using AutoMapper;
using Api.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Api.Controllers;

[ApiController]
[ApiVersion(1)]
[Authorize(Roles = AuthRoles.User)]
[Route("Api/v{v:apiVersion}/User")]
public class UserController(
  UserService userService,
  IMapper mapper
) : Controller {
  [HttpGet]
  [Route("{userId:Guid}")]
  public async Task<ActionResult<UserAdminDto>> GetUser(Guid userId) {
    User? queriedUser = await userService.FindUser(userId);

    if (queriedUser == null) {
      return NotFound();
    }

    var result = mapper.Map<UserAdminDto>(queriedUser);

    return result;
  }

  [Authorize(Roles = AuthRoles.Admin)]
  [HttpGet]
  public async Task<ActionResult<PaginatedResultDto<UserAdminDto>>> GetUsers([FromQuery] PaginationQuery pagination) {
    IQueryable<User> usersQuery = userService.GetUsers();
    int count = await usersQuery.CountAsync();
    List<UserAdminDto> users = await usersQuery
      .OrderByDescending(u => u.CreatedAt)
      .Skip(pagination.Take * pagination.Page)
      .Take(pagination.Take)
      .Select(u => mapper.Map<UserAdminDto>(u))
      .ToListAsync();

    return new PaginatedResultDto<UserAdminDto> {
      Items = users,
      TotalItems = count
    };
  }

  [HttpDelete]
  [Authorize(Roles = AuthRoles.Admin)]
  [Route("{userId:Guid}")]
  public async Task<ActionResult> DeleteUser(Guid userId) {
    User? user = await userService.DeleteUser(userId);
    return user == null ? NotFound() : Ok();
  }

  [HttpPatch]
  [Authorize(Roles = AuthRoles.Admin)]
  [Route("{userId:Guid}")]
  public async Task<ActionResult> UpdateUser(Guid userId, [FromBody] UpdateUserDto updateDto) {
    var user = await userService.FindUser(userId);

    if (user == null) {
      return NotFound();
    }

    await userService.UpdateUser(user, updateDto);
    return NoContent();
  }

  [HttpPost]
  [Authorize(Roles = AuthRoles.SuperAdmin)]
  public async Task<ActionResult> CreateUser([FromBody] CreateUserDto createDto) {
    User? existingUser = await userService.FindUserByUsername(createDto.Username);

    if (existingUser != null)
      return Conflict();

    await userService.CreateUser(createDto);
    return Created();
  }
  
  
  [HttpPost]
  [Route("Clear-Favorites")]
  [Authorize(Roles = AuthRoles.User)]
  public async Task<ActionResult> ClearFavorites() {
    Guid userId = Guid.Parse(User.FindFirst(JwtRegisteredClaimNames.Sub)!.Value);
    User user = (await userService.FindUser(userId, includeFavorites: true))!;
    
    return await userService.ClearFavorites(user);
  }
}
