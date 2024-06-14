using AutoMapper;
using FafCarsApi.Dtos;
using FafCarsApi.Models;
using Microsoft.EntityFrameworkCore;
using ImageHelper = FafCarsApi.Utilities.ImageHelper;

namespace FafCarsApi.Services;

public class ListingService(
  FafCarsDbContext dbContext,
  ILogger<ListingService> logger
) {
  public async Task<Listing?> FindListing(Guid listingId) {
    return await dbContext.Listings.FindAsync(listingId);
  }

  public IQueryable<Listing> GetActiveListings() {
    return dbContext.Listings
      .Include(l => l.Publisher)
      .Where(l => l.DeletedAt == null);
  }

  public IQueryable<Listing> GetInactiveListings() {
    return dbContext.Listings
      .Include(l => l.Publisher)
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
    var listing = new Listing();

    var config = new MapperConfiguration(c =>
      c.CreateMap<CreateListingDto, Listing>()
        .ForMember(
          dest => dest.Images,
          opt => opt.Ignore()
        )
        .ForMember(
          dest => dest.Preview,
          opt => opt.Ignore()
        )
    );
    var mapper = config.CreateMapper();
    mapper.Map(createDto, listing);

    if (createDto.Preview != null) {
      var (_, base64Body) = createDto.Preview;
      string generatedFileName = Guid.NewGuid() + ".webp";
      await ImageHelper.Create(generatedFileName, base64Body);
      listing.Preview = generatedFileName;
    }

    foreach (var image in createDto.Images) {
      var (_, base64Body) = image;
      string generatedFileName = Guid.NewGuid() + ".webp";
      await ImageHelper.Create(generatedFileName, base64Body);
      listing.Images.Add(generatedFileName);
    }

    var publisher = (await dbContext.Users.FindAsync(publisherId))!;
    listing.Publisher = publisher;
    listing.PublisherId = publisher.Id;

    await dbContext.Listings.AddAsync(listing);
    await dbContext.SaveChangesAsync();
  }

  public async Task<IList<Listing>> GetUserFavoriteListings(Guid userId) {
    return await dbContext.Users
      .AsNoTracking()
      .Where(u => u.Id == userId)
      .SelectMany(u => u.Favorites)
      .ToListAsync();
  }
}
