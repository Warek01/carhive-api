using System.IdentityModel.Tokens.Jwt;
using Asp.Versioning;
using AutoMapper;
using FafCarsApi.Dtos.Request;
using FafCarsApi.Dtos.Response;
using FafCarsApi.Enums;
using FafCarsApi.Helpers;
using FafCarsApi.Models;
using FafCarsApi.Queries;
using FafCarsApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FafCarsApi.Controllers;

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
  public async Task<ActionResult<PaginatedResultDto<ListingDto>>> GetListings(
    [FromQuery] ListingQuery query
  ) {
    PaginatedResultDto<ListingDto> listings = await listingService.GetFilteredListingsAsync(query);
    return listings;
  }

  [HttpGet]
  [Route("{listingId:guid}")]
  public async Task<ActionResult<ListingDto>> GetListingDetails(Guid listingId) {
    Listing? listing = await listingService.GetListing(listingId);
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

  [HttpDelete]
  [Authorize(Roles = AuthRoles.Admin)]
  [Route("{listingId:guid}")]
  public async Task<ActionResult> DeleteListing(Guid listingId) {
    var listing = await listingService.GetListing(listingId);
    if (listing == null) return NotFound();
    var publisherId = Guid.Parse(User.FindFirst(JwtRegisteredClaimNames.Sub)!.Value);

    if (User.IsInRole("Admin") || (User.IsInRole("RemoveListing") && publisherId == listingId)) {
      await listingService.DeleteListing(listing);
      return Ok();
    }

    return Unauthorized();
  }

  [HttpPatch]
  [Authorize(Roles = AuthRoles.User)]
  [Route("{listingId:guid}")]
  public ActionResult UpdateListing(Guid listingId, [FromBody] UpdateListingDto updateDto) {
    return Ok();
    // if (User.IsInRole("Admin") || User.IsInRole("ListingCreator")) {
    //   var listing = await listingService.GetListing(listingId);
    //   if (listing == null) return NotFound();
    //   await listingService.UpdateListing(listing, updateDto);
    //   return Ok();
    // }
    //
    // return Unauthorized();
  }

  [Authorize(Roles = AuthRoles.User)]
  [HttpPost]
  public async Task<ActionResult> CreateListing([FromForm] CreateListingDto createDto) {
    var publisherId = Guid.Parse(User.FindFirst(JwtRegisteredClaimNames.Sub)!.Value);
    await listingService.CreateListing(createDto, publisherId);
    return Created();
  }

  [HttpPost]
  [Route("{id:guid}/Add-To-Favorites")]
  [Authorize(Roles = AuthRoles.User)]
  public async Task<ActionResult> AddToFavorites(Guid id) {
    Guid userId = Guid.Parse(User.FindFirst(JwtRegisteredClaimNames.Sub)!.Value);
    User user = (await userService.FindUser(userId, includeFavorites: true))!;

    return await listingService.AddToFavorites(user, id);
  }

  [HttpPost]
  [Route("{id:guid}/Remove-From-Favorites")]
  [Authorize(Roles = AuthRoles.User)]
  public async Task<ActionResult> RemoveFromFavorites(Guid id) {
    Guid userId = Guid.Parse(User.FindFirst(JwtRegisteredClaimNames.Sub)!.Value);
    User user = (await userService.FindUser(userId, includeFavorites: true))!;

    return await listingService.RemoveFromFavorites(user, id);
  }

  [HttpPost]
  [Route("{id:guid}/Set-Status-Sold")]
  [Authorize(Roles = AuthRoles.User)]
  public Task<ActionResult> SetStatusSold(Guid id) {
    return SetStatusAction(id, ListingStatus.Sold);
  }

  [HttpPost]
  [Route("{id:guid}/Set-Status-Deleted")]
  [Authorize(Roles = AuthRoles.User)]
  public Task<ActionResult> SetStatusDeleted(Guid id) {
    return SetStatusAction(id, ListingStatus.Deleted);
  }

  [HttpPost]
  [Route("{id:guid}/Set-Status-Blocked")]
  [Authorize(Roles = AuthRoles.Admin)]
  public Task<ActionResult> SetStatusBlocked(Guid id) {
    return SetStatusAction(id, ListingStatus.Blocked);
  }

  [HttpPost]
  [Route("{id:guid}/Set-Status-Available")]
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
