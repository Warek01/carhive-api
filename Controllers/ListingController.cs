using Asp.Versioning;
using FafCarsApi.Models;
using FafCarsApi.Models.Dto;
using FafCarsApi.Services;
using Microsoft.AspNetCore.Mvc;

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
  public async Task<PaginatedResultDto<ListingDto>> GetListings(
    [FromQuery] PaginationQuery pagination
  )
  {
    return await _listingService.GetListings(pagination);
  }

  [HttpDelete]
  [Route("{listingId:guid}")]
  public async Task<OperationResultDto> DeleteListing(Guid listingId)
  {
    return await _listingService.DeleteListing(listingId);
  }
}
