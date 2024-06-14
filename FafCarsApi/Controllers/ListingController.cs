using System.IdentityModel.Tokens.Jwt;
using Asp.Versioning;
using AutoMapper;
using FafCarsApi.Dtos;
using FafCarsApi.Enums;
using FafCarsApi.Models;
using FafCarsApi.Services;
using FafCarsApi.Utilities;
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
    [FromQuery] PaginationQuery pagination,
    [FromQuery] string[] carTypes
  ) {
    IQueryable<Listing> listings = listingService.GetActiveListings();

    if (carTypes.Length > 0)
      listings = listings.Where(l => carTypes.Contains(l.Type));

    int totalListings = await listings.CountAsync();

    if (pagination.Order != null)
      listings = pagination.Order switch {
        "createdAtDesc" => listings.OrderByDescending(l => l.CreatedAt),
        "createdAtAsc" => listings.OrderBy(l => l.CreatedAt),
        "priceDesc" => listings.OrderByDescending(l => l.Price),
        "priceAsc" => listings.OrderBy(l => l.Price),
        _ => listings.OrderByDescending(l => l.CreatedAt)
      };

    listings = listings
      .Skip(pagination.Page * pagination.Take)
      .Take(pagination.Take);

    var result = new PaginatedResultDto<ListingDto> {
      Items = await listings.Select(l => mapper.Map<ListingDto>(l)).ToListAsync(),
      TotalItems = totalListings
    };

    return Ok(result);
  }

  [HttpGet]
  [Route("{listingId:guid}")]
  public async Task<ActionResult> GetListingDetails(Guid listingId) {
    Listing? listing = await listingService.GetListing(listingId);
    if (listing == null)
      return NotFound();

    ListingDto dto = mapper.Map<ListingDto>(listing);

    if (User.Identity is { IsAuthenticated: true }) {
      Guid userId = Guid.Parse(User.FindFirst(JwtRegisteredClaimNames.Sub)!.Value);
      IList<Listing> favorites = await listingService.GetUserFavoriteListings(userId);
      dto.IsFavorite = favorites.Any(l => l.Id == listing.Id);
    }

    return Ok(dto);
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

  [HttpGet("Favorites")]
  [Authorize]
  public async Task<ActionResult> GetFavorites([FromQuery] PaginationQuery pagination) {
    Guid userId = Guid.Parse(User.FindFirst(JwtRegisteredClaimNames.Sub)!.Value);
    User? user = await userService.FindUser(userId);

    if (user == null)
      return new NotFoundResult();

    IList<Listing> listings = user.Favorites;
    int totalItems = listings.Count;
    IOrderedEnumerable<Listing> orderedListings = pagination.Order switch {
      "createdAtDesc" => listings.OrderByDescending(l => l.CreatedAt),
      "createdAtAsc" => listings.OrderBy(l => l.CreatedAt),
      "priceDesc" => listings.OrderByDescending(l => l.Price),
      "priceAsc" => listings.OrderBy(l => l.Price),
      _ => listings.OrderByDescending(l => l.CreatedAt)
    };
    List<ListingDto> items = orderedListings
      .Skip(pagination.Page * pagination.Take)
      .Take(pagination.Take)
      .Select(mapper.Map<ListingDto>)
      .ToList();
    var response = new PaginatedResultDto<ListingDto> {
      Items = items,
      TotalItems = totalItems,
    };

    return Ok(response);
  }
}
