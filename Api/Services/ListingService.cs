using Api.Data;
using Api.Dtos.Request;
using Api.Dtos.Response;
using Api.Enums;
using Api.Exceptions;
using Api.Helpers;
using Api.Models;
using Api.Queries;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ImageHelper = Api.Helpers.ImageHelper;

namespace Api.Services;

public class ListingService(
  CarHiveDbContext dbContext,
  IMapper mapper
) {
  public async Task<ActionResult<PaginatedResultDto<ListingDto>>> GetFilteredListingsAsync(ListingQuery query) {
    IQueryable<Listing> listings;

    #region Filtering

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

    if (query is { PriceMin: not null, PriceMax: not null } && query.PriceMin > query.PriceMax) {
      throw new BadRequestException("min price cannot be greater then max perice");
    }

    if (query.PriceMin != null) {
      listings = listings.Where(l => l.Price >= query.PriceMin);
    }

    if (query.PriceMax != null) {
      listings = listings.Where(l => l.Price <= query.PriceMax);
    }

    if (query is { MileageMin: not null, MileageMax: not null } && query.MileageMin > query.MileageMax) {
      throw new BadRequestException("min mileage cannot be greater then max mileage");
    }

    if (query.MileageMin != null) {
      listings = listings.Where(l => l.Mileage >= query.MileageMin);
    }

    if (query.MileageMax != null) {
      listings = listings.Where(l => l.Mileage <= query.MileageMax);
    }

    if (query is { WheelSizeMin: not null, WheelSizeMax: not null } && query.WheelSizeMin > query.WheelSizeMax) {
      throw new BadRequestException("min wheel size cannot be greater then max wheel size");
    }

    if (query.WheelSizeMin != null) {
      listings = listings.Where(l => l.WheelSize >= query.WheelSizeMin);
    }

    if (query.WheelSizeMax != null) {
      listings = listings.Where(l => l.WheelSize <= query.WheelSizeMax);
    }

    if (query is { ClearanceMin: not null, ClearanceMax: not null } && query.ClearanceMin > query.ClearanceMax) {
      throw new BadRequestException("min clearance cannot be greater then max clearance");
    }

    foreach (string brand in query.BrandNames) {
      listings = listings.Where(l => l.BrandName == brand);
    }

    foreach (string model in query.ModelNames) {
      listings = listings.Where(l => l.ModelName == model);
    }

    foreach (CarBodyStyle bodyStyle in query.BodyStyles) {
      listings = listings.Where(l => l.BodyStyle == bodyStyle);
    }

    foreach (CarFuelType fuelType in query.FuelTypes) {
      listings = listings.Where(l => l.FuelType == fuelType);
    }

    foreach (CarTransmission transmission in query.Transmissions) {
      listings = listings.Where(l => l.Transmission == transmission);
    }

    foreach (CarDrivetrain drivetrain in query.Drivetrains) {
      listings = listings.Where(l => l.Drivetrain == drivetrain);
    }

    foreach (CarColor color in query.Colors) {
      listings = listings.Where(l => l.Color == color);
    }
    
    foreach (CarStatus status in query.Statuses) {
      listings = listings.Where(l => l.CarStatus == status);
    }

    if (query.CountryCode != null) {
      listings = listings.Where(l => l.Country.Code == query.CountryCode);
    }

    if (query.CityName != null) {
      listings = listings.Where(l => l.CityName == query.CityName);
    }

    if (query.ClearanceMin != null) {
      listings = listings.Where(l => l.Clearance >= query.ClearanceMin);
    }

    if (query.ClearanceMax != null) {
      listings = listings.Where(l => l.Clearance <= query.ClearanceMax);
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

    #endregion

    int totalListings = await listings.CountAsync();

    listings = listings
      .Skip(query.Page * query.Take)
      .Take(query.Take);

    return new PaginatedResultDto<ListingDto> {
      Items = await listings.Select(l => mapper.Map<ListingDto>(l)).ToListAsync(),
      TotalItems = totalListings
    };
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
    listing.Status = ListingStatus.Deleted;
    var activity = new ListingActivity {
      Type = ListingAction.Delete,
      Listing = listing,
    };

    await dbContext.ListingActivities.AddAsync(activity);
    await dbContext.SaveChangesAsync();
  }

  public async Task RestoreListing(Listing listing) {
    listing.DeletedAt = null;
    listing.Status = ListingStatus.Available;

    var activity = new ListingActivity {
      Type = ListingAction.Restore,
      Listing = listing,
    };
    await dbContext.ListingActivities.AddAsync(activity);
    await dbContext.SaveChangesAsync();
  }

  public async Task UpdateListing(Listing listing, UpdateListingDto updateDto) {
    mapper.Map(updateDto, listing);
    listing.UpdatedAt = DateTime.UtcNow;

    var activity = new ListingActivity {
      Type = ListingAction.Update,
      Listing = listing,
    };

    await dbContext.ListingActivities.AddAsync(activity);
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

    var activity = new ListingActivity {
      Type = ListingAction.Create,
      Listing = listing,
    };

    await dbContext.ListingActivities.AddAsync(activity);
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

    var activity = new ListingActivity {
      Type = ListingAction.AddToFavorites,
      ListingId = id,
      User = user,
    };

    await dbContext.ListingActivities.AddAsync(activity);
    await dbContext.SaveChangesAsync();
    return new OkResult();
  }

  public async Task<ActionResult> RemoveFromFavorites(User user, Guid id) {
    Listing? listing = await GetListing(id);

    if (listing == null) {
      return new NotFoundObjectResult("listing not found");
    }

    user.Favorites.Remove(listing);

    var activity = new ListingActivity {
      Type = ListingAction.RemoveFromFavorites,
      ListingId = id,
      User = user,
    };

    await dbContext.ListingActivities.AddAsync(activity);
    await dbContext.SaveChangesAsync();
    return new OkResult();
  }

  public async Task SetListingStatus(Listing listing, ListingStatus status) {
    listing.Status = status;
    await dbContext.SaveChangesAsync();
  }

  public async Task<ActionResult> IncrementViews(Guid id) {
    Listing? listing = await FindListing(id);

    if (listing == null) {
      return new NotFoundResult();
    }

    listing.Views++;
    await dbContext.SaveChangesAsync();
    
    return new OkResult();
  }
}
