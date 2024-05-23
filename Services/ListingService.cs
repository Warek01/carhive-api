using System.Text.RegularExpressions;
using AutoMapper;
using FafCarsApi.Models;
using FafCarsApi.Models.Dto;
using FafCarsApi.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace FafCarsApi.Services;

public partial class ListingService(
  FafCarsDbContext dbContext,
  ILogger<ListingService> logger,
  StaticFileService fileService) {
  public async Task<int> GetActiveListingsCount() {
    return await dbContext.Listings
      .CountAsync(l => l.DeletedAt == null);
  }

  public async Task<int> GetDeletedListingsCount() {
    return await dbContext.Listings
      .CountAsync(l => l.DeletedAt != null);
  }

  public IQueryable<Listing> GetActiveListings() {
    return dbContext.Listings
      .Include(l => l.Publisher);
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
    IMapper mapper = config.CreateMapper();
    mapper.Map(updateDto, listing);
    listing.UpdatedAt = DateTime.UtcNow;
    await dbContext.SaveChangesAsync();
  }

  public async Task CreateListing(CreateListingDto createDto, Guid publisherId) {
    var listing = new Listing();

    if (createDto.Preview != null) {
      string generatedFileName = await CreateImage(createDto.Preview.FileName, createDto.Preview.Base64Body);
      listing.Preview = generatedFileName;
    }

    foreach (FileDto image in createDto.Images) {
      string generatedFileName = await CreateImage(image.FileName, image.Base64Body);
      listing.Images.Add(generatedFileName);
    }

    var config = new MapperConfiguration(c =>
      c.CreateMap<CreateListingDto, Listing>()
        .ForMember(
          dest => dest.Images,
          opt => opt.Ignore()
        )
    );
    IMapper mapper = config.CreateMapper();
    mapper.Map(createDto, listing);

    User publisher = (await dbContext.Users.FindAsync(publisherId))!;
    listing.Publisher = publisher;
    listing.PublisherId = publisher.Id;

    await dbContext.Listings.AddAsync(listing);
    await dbContext.SaveChangesAsync();
  }

  private static async Task<string> CreateImage(string fileName, string base64Body) {
    var regex = Base64ImagePrefixRegex();

    string b64 = regex.Replace(
      base64Body,
      string.Empty
    );

    string generatedFileName = $"{Guid.NewGuid()}.{Path.GetExtension(fileName)[1..]}";

    await StaticFileService.Create(generatedFileName, b64);
    return generatedFileName;
  }

  [GeneratedRegex(@"^(data:image\/[a-zA-Z]+;base64,|base64,)", RegexOptions.IgnoreCase)]
  private static partial Regex Base64ImagePrefixRegex();
}
