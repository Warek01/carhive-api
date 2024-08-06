using System.IdentityModel.Tokens.Jwt;
using Api.Dtos.Request;
using Api.Dtos.Response;
using Api.Enums;
using Api.Helpers;
using Api.Models;
using Api.Queries;
using Api.Services;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers;

[ApiController]
[ApiVersion(1)]
[AllowAnonymous]
[Route("Api/v{v:apiVersion}/Listing")]
public class ListingController(
  ListingService listingService,
  IMapper mapper,
  UserService userService
) : Controller {
  [HttpGet]
  public Task<ActionResult<PaginatedResultDto<ListingDto>>> GetListings(
    [FromQuery] ListingQuery query
  ) {
    return listingService.GetFilteredListingsAsync(query);
  }
  
  [HttpPost]
  [Authorize(Roles = AuthRoles.User)]
  public async Task<ActionResult> CreateListing([FromForm] CreateListingDto createDto) {
    var publisherId = Guid.Parse(User.FindFirst(JwtRegisteredClaimNames.Sub)!.Value);
    await listingService.CreateListing(createDto, publisherId);
    return Created();
  }

  [HttpGet("Count")]
  public async Task<ActionResult<int>> GetTotalListingsCount() {
    return await listingService.GetTotalListings().CountAsync();
  }

  [HttpGet("{id:guid}")]
  public async Task<ActionResult<ListingDto>> GetListingDetails(Guid id) {
    Listing? listing = await listingService.GetListing(id);
    if (listing == null)
      return NotFound();

    ListingDto dto = mapper.Map<ListingDto>(listing);

    if (User.Identity is { IsAuthenticated: true }) {
      Guid userId = Guid.Parse(User.FindFirst(JwtRegisteredClaimNames.Sub)!.Value);
      List<Listing> favorites = await listingService.GetUserFavoriteListings(userId).ToListAsync();
      dto.IsFavorite = favorites.Any(l => l.Id == listing.Id);
    }

    return dto;
  }

  [HttpDelete("{id:guid}")]
  [Authorize(Roles = AuthRoles.Admin)]
  public async Task<ActionResult> DeleteListing(Guid id) {
    var listing = await listingService.GetListing(id);
    if (listing == null) return NotFound();
    var publisherId = Guid.Parse(User.FindFirst(JwtRegisteredClaimNames.Sub)!.Value);

    if (User.IsInRole("Admin") || (User.IsInRole("RemoveListing") && publisherId == id)) {
      await listingService.DeleteListing(listing);
      return Ok();
    }

    return Unauthorized();
  }

  [HttpPatch("{id:guid}")]
  [Authorize(Roles = AuthRoles.User)]
  public ActionResult UpdateListing(Guid id, [FromBody] UpdateListingDto updateDto) {
    return Ok();
    // if (User.IsInRole("Admin") || User.IsInRole("ListingCreator")) {
    //   var listing = await listingService.GetListing(id);
    //   if (listing == null) return NotFound();
    //   await listingService.UpdateListing(listing, updateDto);
    //   return Ok();
    // }
    //
    // return Unauthorized();
  }

  [HttpPost("{id:guid}/Add-To-Favorites")]
  [Authorize(Roles = AuthRoles.User)]
  public async Task<ActionResult> AddToFavorites(Guid id) {
    Guid userId = Guid.Parse(User.FindFirst(JwtRegisteredClaimNames.Sub)!.Value);
    User user = (await userService.FindUser(userId, includeFavorites: true))!;

    return await listingService.AddToFavorites(user, id);
  }

  [HttpPost("{id:guid}/Remove-From-Favorites")]
  [Authorize(Roles = AuthRoles.User)]
  public async Task<ActionResult> RemoveFromFavorites(Guid id) {
    Guid userId = Guid.Parse(User.FindFirst(JwtRegisteredClaimNames.Sub)!.Value);
    User user = (await userService.FindUser(userId, includeFavorites: true))!;

    return await listingService.RemoveFromFavorites(user, id);
  }

  [HttpPost("{id:guid}/Increment-Views")]
  public Task<ActionResult> IncrementViews(Guid id) {
    return listingService.IncrementViews(id);
  }
  
  [HttpPost("{id:guid}/Set-Status-Sold")]
  [Authorize(Roles = AuthRoles.User)]
  public Task<ActionResult> SetStatusSold(Guid id) {
    return SetStatusAction(id, ListingStatus.Sold);
  }

  [HttpPost("{id:guid}/Set-Status-Deleted")]
  [Authorize(Roles = AuthRoles.User)]
  public Task<ActionResult> SetStatusDeleted(Guid id) {
    return SetStatusAction(id, ListingStatus.Deleted);
  }

  [HttpPost("{id:guid}/Set-Status-Blocked")]
  [Authorize(Roles = AuthRoles.Admin)]
  public Task<ActionResult> SetStatusBlocked(Guid id) {
    return SetStatusAction(id, ListingStatus.Blocked);
  }

  [HttpPost("{id:guid}/Set-Status-Available")]
  [Authorize(Roles = AuthRoles.Admin)]
  public Task<ActionResult> SetStatusRestored(Guid id) {
    return SetStatusAction(id, ListingStatus.Available);
  }

  private async Task<ActionResult> SetStatusAction(Guid id, ListingStatus status) {
    Guid userId = Guid.Parse(User.FindFirst(JwtRegisteredClaimNames.Sub)!.Value);
    User user = (await userService.FindUser(userId, includeFavorites: true))!;
    Listing? listing = await listingService.FindListing(id);

    if (listing == null) {
      return NotFound("listing not found");
    }
    
    if (!User.IsInRole(AuthRoles.Admin) && !user.Listings.Contains(listing)) {
      return Forbid("user does not own listing");
    }

    await listingService.SetListingStatus(listing, status);
    return Ok();
  }
}
