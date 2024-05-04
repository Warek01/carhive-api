using FafCarsApi.Models;
using FafCarsApi.Models.Dto;
using FafCarsApi.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace FafCarsApi.Services;

public class ListingService
{
  private readonly FafCarsDbContext _dbContext;

  public ListingService(FafCarsDbContext dbContext)
  {
    _dbContext = dbContext;
  }

  public async Task<PaginatedResultDto<ListingDto>> GetListings(PaginationQuery pagination)
  {
    int totalPages = await _dbContext.Listings.CountAsync();

    ICollection<ListingDto> listings = await _dbContext.Listings
      .Skip(pagination.Page * pagination.Take)
      .Take(pagination.Take)
      .Include(l => l.Publisher)
      .Select(l => new ListingDto
      {
        Id = l.Id,
        BrandName = l.BrandName,
        ModelName = l.ModelName,
        Clearance = l.Clearance,
        Horsepower = l.Horsepower,
        EngineType = l.EngineType,
        EngineVolume = l.EngineVolume,
        WheelSize = l.WheelSize,
        Color = l.Color,
        Mileage = l.Mileage,
        Price = l.Price,
        Type = l.Type,
        Year = l.Year,
        Publisher = new UserDto
        {
          Id = l.Publisher.Id,
          Username = l.Publisher.Username
        }
      })
      .ToListAsync();

    return new PaginatedResultDto<ListingDto>
    {
      Items = listings,
      Page = pagination.Page,
      PageSize = pagination.Take,
      TotalItems = totalPages
    };
  }

  public async Task<OperationResultDto> DeleteListing(Guid listingId)
  {
    Listing? listing = await _dbContext.Listings.FindAsync(listingId);

    if (listing == null)
      return new OperationResultDto
      {
        Success = false,
        Error = "listing not found"
      };

    _dbContext.Remove(listing);
    await _dbContext.SaveChangesAsync();

    return new OperationResultDto
    {
      Success = true
    };
  }
}
