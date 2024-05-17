using System.Security.Claims;
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
[Route("api/v{v:apiVersion}/listing")]
public class ListingController : Controller
{
  private readonly ListingService _listingService;

  public ListingController(ListingService listingService)
  {
    _listingService = listingService;
  }

  [HttpGet]
  public async Task<ActionResult<PaginatedResultDto<ListingDto>>> GetListings(
    [FromQuery] PaginationQuery pagination,
    [FromQuery] string[] carTypes
  )
  {
    IQueryable<Listing> listings = _listingService.GetActiveListings();

    if (carTypes.Length > 0)
      listings = listings.Where(l => carTypes.Contains(l.Type));

    int totalListings = await listings.CountAsync();

    if (pagination.Order != null)
      listings = pagination.Order switch
      {
        "createdAtDesc" => listings.OrderByDescending(l => l.CreatedAt),
        "createdAtAsc" => listings.OrderBy(l => l.CreatedAt),
        "priceDesc" => listings.OrderByDescending(l => l.Price),
        "priceAsc" => listings.OrderBy(l => l.Price),
        _ => listings.OrderByDescending(l => l.CreatedAt),
      };

    listings = listings
      .Skip(pagination.Page * pagination.Take)
      .Take(pagination.Take);

    return Ok(
      new PaginatedResultDto<ListingDto>
      {
        Items = await listings.Select(l => ListingDto.FromListing(l)).ToListAsync(),
        TotalItems = totalListings,
      }
    );
  }

  [HttpGet]
  [Route("{listingId:guid}")]
  public async Task<ActionResult<ListingDto>> GetListingDetails(Guid listingId)
  {
    Listing? listing = await _listingService.GetListing(listingId);
    if (listing == null) return NotFound();

    return Ok(ListingDto.FromListing(listing));
  }

  [HttpDelete]
  [Authorize(Roles = "Admin")]
  [Route("{listingId:guid}")]
  public async Task<ActionResult> DeleteListing(Guid listingId)
  {
    Listing? listing = await _listingService.GetListing(listingId);
    if (listing == null) return NotFound();
    var publisherId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

    if (User.IsInRole("Admin") || (User.IsInRole("RemoveListing") && publisherId == listingId))
    {
      await _listingService.DeleteListing(listing);
      return Ok();
    }
    else
    {
      return Unauthorized();
    }
  }

  [HttpPatch]
  [Authorize(Roles = "Admin,ListingCreator")]
  [Route("{listingId:guid}")]
  public async Task<ActionResult> UpdateListing(Guid listingId, [FromBody] UpdateListingDto updateDto)
  {
    if (User.IsInRole("Admin") || User.IsInRole("ListingCreator"))
    {
      Listing? listing = await _listingService.GetListing(listingId);
      if (listing == null) return NotFound();
      await _listingService.UpdateListing(listing, updateDto);
      return Ok();
    }
    else
    {
      return Unauthorized();
    }
  }

  [Authorize(Roles = "Admin,ListingCreator")]
  [HttpPost]
  public async Task<ActionResult> CreateListing([FromBody] CreateListingDto createDto)
  {
    var publisherId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
    await _listingService.CreateListing(createDto, publisherId);
    return Created();
  }
}
