using Api.Dtos.Request;
using Api.Dtos.Response;
using Api.Helpers;
using Api.Models;
using Api.Queries;
using Api.Services;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Api.Controllers;

[ApiController]
[AllowAnonymous]
[ApiVersion(1)]
[Route("Api/v{version:apiVersion}/Comment")]
public class CommentController(CommentService commentService) : Controller {
  [HttpPost]
  [Authorize(Roles = AuthRoles.User)]
  public async Task<ActionResult> CreateComment([FromBody] CreateCommentDto dto) {
    Guid userId = Guid.Parse(User.FindFirst(JwtRegisteredClaimNames.Sub)!.Value);
    await commentService.CreateComment(dto, userId);
    return Created();
  }

  [HttpGet]
  public async Task<ActionResult<PaginatedResultDto<CommentDto>>> GetComments([FromQuery] CommentQuery query) {
    return await commentService.GetComments(query);
  }

  [HttpDelete("{id:guid}")]
  [Authorize(Roles = AuthRoles.User)]
  public async Task<ActionResult> DeleteComment(Guid id) {
    Comment? comment = await commentService.FindComment(id);

    if (comment == null) {
      return NotFound();
    }

    Guid userId = Guid.Parse(User.FindFirst(JwtRegisteredClaimNames.Sub)!.Value);

    if (comment.UserId != userId) {
      return Unauthorized();
    }

    await commentService.DeleteComment(comment);

    return Ok();
  }
}
