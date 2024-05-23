using System.IdentityModel.Tokens.Jwt;
using Asp.Versioning;
using FafCarsApi.Models;
using FafCarsApi.Models.Dto;
using FafCarsApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FafCarsApi.Controllers;

[ApiController]
[ApiVersion(1)]
[Route("api/v{v:apiVersion}/listing")]
public class ListingController(ListingService listingService) : Controller {
  [HttpGet]
  public async Task<ActionResult<PaginatedResultDto<ListingDto>>> GetListings(
    [FromQuery] PaginationQuery pagination,
    [FromQuery] string[] carTypes
  ) {
    var listings = listingService.GetActiveListings();

    if (carTypes.Length > 0)
      listings = listings.Where(l => carTypes.Contains(l.Type));

    var totalListings = await listings.CountAsync();

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

    return Ok(
      new PaginatedResultDto<ListingDto> {
        Items = await listings.Select(l => ListingDto.FromListing(l)).ToListAsync(),
        TotalItems = totalListings
      }
    );
  }

  [HttpGet]
  [Route("{listingId:guid}")]
  public async Task<ActionResult<ListingDto>> GetListingDetails(Guid listingId) {
    var listing = await listingService.GetListing(listingId);
    if (listing == null) return NotFound();

    return Ok(ListingDto.FromListing(listing));
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
  [Authorize(Roles = "Admin,ListingCreator")]
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

  [Authorize(Roles = "Admin,ListingCreator")]
  [HttpPost]
  public async Task<ActionResult> CreateListing([FromBody] CreateListingDto createDto) {
    var publisherId = Guid.Parse(User.FindFirst(JwtRegisteredClaimNames.Sub)!.Value);
    await listingService.CreateListing(createDto, publisherId);
    return Created();
  }
}