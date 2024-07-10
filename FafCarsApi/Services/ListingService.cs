using AutoMapper;
using FafCarsApi.Data;
using FafCarsApi.Dto;
using FafCarsApi.Exceptions;
using FafCarsApi.Models;
using FafCarsApi.Queries;
using Microsoft.EntityFrameworkCore;
using ImageHelper = FafCarsApi.Helpers.ImageHelper;

namespace FafCarsApi.Services;

public class ListingService(
  FafCarsDbContext dbContext,
  ILogger<ListingService> logger,
  IMapper mapper
) {
  public async Task<PaginatedResultDto<ListingDto>> GetFilteredListingsAsync(ListingsQuery query) {
    IQueryable<Listing> listings;

    if (query.Favorites) {
      if (query.UserId == null) throw new BadRequestException("user id not provided");
      listings = GetUserFavoriteListings(query.UserId.Value);
    }
    else {
      listings = GetActiveListings();
    }

    if (query.UserId != null)
      listings = listings.Where(l => l.PublisherId == query.UserId);

    if (query.BodyStyles?.Count > 0)
      listings = listings.Where(l => query.BodyStyles.Contains(l.BodyStyle));

    if (query is { PriceMin: not null, PriceMax: not null } && query.PriceMin > query.PriceMax)
      throw new BadRequestException("min price cannot be greater then max perice");

    if (query.PriceMin != null)
      listings = listings.Where(l => l.Price >= query.PriceMin);

    if (query.PriceMax != null)
      listings = listings.Where(l => l.Price <= query.PriceMax);

    if (query.BrandNames != null)
      foreach (string brand in query.BrandNames)
        listings = listings.Where(l => l.Brand.Name == brand);

    if (query.CountryCode != null)
      listings = listings.Where(l => l.Country.Code == query.CountryCode);

    if (query.EngineTypes != null)
      listings = listings.Where(l => query.EngineTypes.Contains(l.FuelType));

    // TODO: perform fuzzy search
    if (query.Address != null)
      listings = listings.Where(
        l => l.SellAddress != null && l.SellAddress.ToLower().Contains(query.Address.ToLower())
      );

    if (query.Order != null)
      listings = query.Order switch {
        "createdAtDesc" => listings.OrderByDescending(l => l.CreatedAt),
        "createdAtAsc" => listings.OrderBy(l => l.CreatedAt),
        "priceDesc" => listings.OrderByDescending(l => l.Price),
        "priceAsc" => listings.OrderBy(l => l.Price),
        "yearAsc" => listings.OrderBy(l => l.ProductionYear),
        "yearDesc" => listings.OrderByDescending(l => l.ProductionYear),
        _ => listings.OrderByDescending(l => l.CreatedAt)
      };

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
    var config = new MapperConfiguration(cfg => cfg.CreateMap<UpdateListingDto, Listing>());
    var mapper = config.CreateMapper();
    mapper.Map(updateDto, listing);
    listing.UpdatedAt = DateTime.UtcNow;
    await dbContext.SaveChangesAsync();
  }

  public async Task CreateListing(CreateListingDto createDto, Guid publisherId) {
    var model = await dbContext.Models.FindAsync(createDto.BrandName, createDto.ModelName);
    if (model == null) {
      throw new BadRequestException("model not found");
    }

    Listing listing = mapper.Map<Listing>(createDto);

    var publisher = (await dbContext.Users.FindAsync(publisherId))!;
    listing.Publisher = publisher;
    listing.PublisherId = publisher.Id;

    await dbContext.Listings.AddAsync(listing);
    await dbContext.SaveChangesAsync();

    if (createDto.Preview != null) {
      string generatedFileName = Guid.NewGuid() + ".webp";
      await ImageHelper.Create(generatedFileName, createDto.Preview);
      listing.PreviewFilename = generatedFileName;
    }

    foreach (IFormFile image in createDto.Images) {
      string generatedFileName = Guid.NewGuid() + ".webp";
      await ImageHelper.Create(generatedFileName, image);
      listing.ImagesFilenames.Add(generatedFileName);
    }

    await dbContext.SaveChangesAsync();
  }

  public IQueryable<Listing> GetUserFavoriteListings(Guid userId) {
    return dbContext.Users
      .AsNoTracking()
      .Where(u => u.Id == userId)
      .SelectMany(u => u.Favorites);
  }
}
