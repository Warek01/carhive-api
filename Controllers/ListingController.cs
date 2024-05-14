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
    [FromQuery] PaginationQuery pagination
  )
  {
    IQueryable<Listing> listings = _listingService.GetActiveListings(pagination);
    ICollection<ListingDto> result = await listings
      .Select(l => ListingDto.FromListing(l))
      .ToListAsync();
    int totalListings = await _listingService.GetActiveListingsCount();

    return Ok(
      new PaginatedResultDto<ListingDto>
      {
        Items = result,
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
  [Authorize(Roles = "Admin,RemoveListing")]
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
  [Authorize(Roles = "Admin,CreateListing")]
  [Route("{listingId:guid}")]
  public async Task<ActionResult> UpdateListing(Guid listingId, [FromBody] UpdateListingDto updateDto)
  {
    if (User.IsInRole("Admin") || User.IsInRole("CreateListing"))
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

  [Authorize(Roles = "Admin,CreateListing")]
  [HttpPost]
  public async Task<ActionResult> CreateListing([FromBody] CreateListingDto createDto)
  {
    var publisherId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
    await _listingService.CreateListing(createDto, publisherId);
    return Created();
  }
}
