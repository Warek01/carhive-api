using AutoMapper;
using FafCarsApi.Data;
using FafCarsApi.Dtos.Request;
using FafCarsApi.Dtos.Response;
using FafCarsApi.Enums;
using FafCarsApi.Exceptions;
using FafCarsApi.Models;
using FafCarsApi.Queries;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ImageHelper = FafCarsApi.Helpers.ImageHelper;

namespace FafCarsApi.Services;

public class ListingService(
  FafCarsDbContext dbContext,
  ILogger<ListingService> logger,
  IMapper mapper
) {
  public async Task<ActionResult<PaginatedResultDto<ListingDto>>> GetFilteredListingsAsync(ListingQuery query) {
    IQueryable<Listing> listings;

    if (query.Favorites) {
      if (query.UserId == null) throw new BadRequestException("user id not provided");
      listings = GetUserFavoriteListings(query.UserId.Value);
    }
    else {
      listings = GetActiveListings();
    }

    if (query.UserId != null) {
      listings = listings.Where(l => l.PublisherId == query.UserId);
    }

    if (query.BodyStyles?.Count > 0) {
      listings = listings.Where(l => l.BodyStyle != null && query.BodyStyles.Contains(l.BodyStyle.Value));
    }

    if (query is { PriceMin: not null, PriceMax: not null } && query.PriceMin > query.PriceMax) {
      throw new BadRequestException("min price cannot be greater then max perice");
    }

    if (query.PriceMin != null) {
      listings = listings.Where(l => l.Price >= query.PriceMin);
    }

    if (query.PriceMax != null) {
      listings = listings.Where(l => l.Price <= query.PriceMax);
    }

    if (query.BrandNames != null) {
      foreach (string brand in query.BrandNames) {
        listings = listings.Where(l => l.BrandName == brand);
      }
    }

    if (query.ModelNames != null) {
      foreach (string model in query.ModelNames) {
        listings = listings.Where(l => l.ModelName == model);
      }
    }

    if (query.CountryCode != null) {
      listings = listings.Where(l => l.Country.Code == query.CountryCode);
    }

    if (query.FuelTypes != null) {
      listings = listings.Where(l => l.FuelType != null && query.FuelTypes.Contains(l.FuelType.Value));
    }

    if (query.City != null) {
      listings = listings.Where(l => l.City == query.City);
    }

    if (query.Order != null) {
      listings = query.Order switch {
        "createdAtDesc" => listings.OrderByDescending(l => l.CreatedAt),
        "createdAtAsc" => listings.OrderBy(l => l.CreatedAt),
        "priceDesc" => listings.OrderByDescending(l => l.Price),
        "priceAsc" => listings.OrderBy(l => l.Price),
        "yearAsc" => listings.OrderBy(l => l.ProductionYear),
        "yearDesc" => listings.OrderByDescending(l => l.ProductionYear),
        _ => listings.OrderByDescending(l => l.CreatedAt)
      };
    }

    int totalListings = await listings.CountAsync();

    listings = listings
      .Skip(query.Page * query.Take)
      .Take(query.Take);

    var result = new PaginatedResultDto<ListingDto> {
      Items = await listings.Select(l => mapper.Map<ListingDto>(l)).ToListAsync(),
      TotalItems = totalListings
    };

    return new OkObjectResult(result);
  }

  public async Task<Listing?> FindListing(Guid listingId) {
    return await dbContext.Listings.FindAsync(listingId);
  }

  // Total registered listings, regardless of their status
  public IQueryable<Listing> GetTotalListings() {
    return dbContext.Listings.AsNoTracking();
  }

  public IQueryable<Listing> GetActiveListings() {
    return dbContext.Listings
      .AsNoTracking()
      .Include(l => l.Publisher)
      .Where(l => l.DeletedAt == null);
  }

  public IQueryable<Listing> GetInactiveListings() {
    return dbContext.Listings
      .AsNoTracking()
      .Include(l => l.Publisher)
      .Include(l => l.Brand)
      .Include(l => l.Country)
      .Where(l => l.DeletedAt != null);
  }

  public async Task<Listing?> GetListing(Guid listingId) {
    return await dbContext.Listings
      .Include(l => l.Publisher)
      .Where(l => l.Id == listingId)
      .FirstOrDefaultAsync();
  }

  public async Task DeleteListing(Listing listing) {
    listing.DeletedAt = DateTime.UtcNow;
    await dbContext.SaveChangesAsync();
    logger.LogInformation($"Deleted listing {listing.Id}");
  }

  public async Task RestoreListing(Listing listing) {
    listing.DeletedAt = null;
    await dbContext.SaveChangesAsync();
    logger.LogInformation($"Restored listing {listing.Id}");
  }

  public async Task UpdateListing(Listing listing, UpdateListingDto updateDto) {
    mapper.Map(updateDto, listing);
    listing.UpdatedAt = DateTime.UtcNow;
    await dbContext.SaveChangesAsync();
  }

  public async Task CreateListing(CreateListingDto createDto, Guid publisherId) {
    var model = await dbContext.Models.FindAsync(createDto.ModelName, createDto.BrandName);
    if (model == null) {
      throw new BadRequestException("model not found");
    }

    Listing listing = mapper.Map<Listing>(createDto);
    var publisher = (await dbContext.Users.FindAsync(publisherId))!;
    listing.Publisher = publisher;
    listing.PublisherId = publisher.Id;

    foreach (IFormFile image in createDto.Images) {
      string generatedFileName = Guid.NewGuid() + ".webp";
      await ImageHelper.Create(generatedFileName, image);
      listing.Images.Add(generatedFileName);
    }

    await dbContext.Listings.AddAsync(listing);
    await dbContext.SaveChangesAsync();
  }

  public IQueryable<Listing> GetUserFavoriteListings(Guid userId) {
    return dbContext.Users
      .AsNoTracking()
      .Where(u => u.Id == userId)
      .SelectMany(u => u.Favorites);
  }

  public async Task<ActionResult> AddToFavorites(User user, Guid id) {
    Listing? listing = await GetListing(id);

    if (listing == null) {
      return new NotFoundObjectResult("listing not found");
    }

    if (user.Favorites.Contains(listing)) {
      return new OkResult();
    }

    user.Favorites.Add(listing);
    await dbContext.SaveChangesAsync();
    return new OkResult();
  }

  public async Task<ActionResult> RemoveFromFavorites(User user, Guid id) {
    Listing? listing = await GetListing(id);

    if (listing == null) {
      return new NotFoundObjectResult("listing not found");
    }

    user.Favorites.Remove(listing);
    await dbContext.SaveChangesAsync();
    return new OkResult();
  }

  public async Task SetListingStatus(Listing listing, ListingStatus status) {
    listing.Status = status;
    await dbContext.SaveChangesAsync();
  }
}
