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
[AllowAnonymous]
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
      .Select(l => ListingDto.FromListingWithPublisher(l))
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

    return Ok(ListingDto.FromListingWithPublisher(listing));
  }

  [HttpDelete]
  [Route("{listingId:guid}")]
  public async Task<ActionResult> DeleteListing(Guid listingId)
  {
    Listing? listing = await _listingService.GetListing(listingId);
    if (listing == null) return NotFound();
    await _listingService.DeleteListing(listing);
    return Ok();
  }

  [HttpPatch]
  [Route("{listingId:guid}")]
  public async Task<ActionResult> UpdateListing(Guid listingId, [FromBody] UpdateListingDto updateDto)
  {
    Listing? listing = await _listingService.GetListing(listingId);
    if (listing == null) return NotFound();
    await _listingService.UpdateListing(listing, updateDto);
    return Ok();
  }

  [HttpPost]
  public async Task<ActionResult> CreateListing([FromBody] CreateListingDto createDto)
  {
    await _listingService.CreateListing(createDto);
    return Created();
  }
}
