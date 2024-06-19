using System.IdentityModel.Tokens.Jwt;
using Asp.Versioning;
using AutoMapper;
using FafCarsApi.Dtos;
using FafCarsApi.Enums;
using FafCarsApi.Models;
using FafCarsApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FafCarsApi.Controllers;

[ApiController]
[ApiVersion(1)]
[Route("Api/v{v:apiVersion}/[controller]")]
public class ListingController(
  ListingService listingService,
  IMapper mapper,
  UserService userService
) : Controller {
  [HttpGet]
  public async Task<ActionResult<PaginatedResultDto<ListingDto>>> GetListings(
    [FromQuery] ListingsQueryDto query
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
  [Authorize(Roles = "Admin")]
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
  [Authorize(Roles = "Admin, ListingCreator")]
  [Route("{listingId:guid}")]
  public async Task<ActionResult> UpdateListing(Guid listingId, [FromBody] UpdateListingDto updateDto) {
    if (User.IsInRole("Admin") || User.IsInRole("ListingCreator")) {
      var listing = await listingService.GetListing(listingId);
      if (listing == null) return NotFound();
      await listingService.UpdateListing(listing, updateDto);
      return Ok();
    }

    return Unauthorized();
  }

  [Authorize(Roles = "Admin, ListingCreator")]
  [HttpPost]
  public async Task<ActionResult> CreateListing([FromBody] CreateListingDto createDto) {
    var publisherId = Guid.Parse(User.FindFirst(JwtRegisteredClaimNames.Sub)!.Value);
    await listingService.CreateListing(createDto, publisherId);
    return Created();
  }

  [Authorize]
  [HttpPost("Favorites")]
  public async Task<IActionResult> UpdateFavorites([FromBody] FavoriteListingActionDto actionDto) {
    Guid userId = Guid.Parse(User.FindFirst(JwtRegisteredClaimNames.Sub)!.Value);
    User? user = await userService.FindUser(userId, includeFavorites: true);

    if (user == null)
      return new NotFoundResult();

    Listing? listing = null;

    if (actionDto.Type is FavoriteListingAction.Add or FavoriteListingAction.Remove) {
      if (actionDto.ListingId == null)
        return BadRequest("listing id not provided");

      listing = await listingService.FindListing(actionDto.ListingId.Value);

      if (listing == null)
        return NotFound();
    }

    switch (actionDto.Type) {
      case FavoriteListingAction.Add:
        await userService.AddListingToFavorites(user, listing!);
        break;
      case FavoriteListingAction.Remove:
        await userService.RemoveListingFromFavorites(user, listing!);
        break;
      case FavoriteListingAction.RemoveAll:
        await userService.ClearFavorites(user);
        break;
      default:
        return BadRequest("unknown action");
    }

    return Ok();
  }
}
