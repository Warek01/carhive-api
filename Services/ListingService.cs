﻿using System.Text.RegularExpressions;
using AutoMapper;
using FafCarsApi.Models;
using FafCarsApi.Models.Dto;
using FafCarsApi.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace FafCarsApi.Services;

public class ListingService
{
  private readonly FafCarsDbContext _dbContext;
  private readonly ILogger<ListingService> _logger;
  private readonly StaticFileService _fileService;

  public ListingService(
    FafCarsDbContext dbContext,
    ILogger<ListingService> logger,
    StaticFileService fileService
  )
  {
    _dbContext = dbContext;
    _logger = logger;
    _fileService = fileService;
  }

  public async Task<int> GetActiveListingsCount()
  {
    return await _dbContext.Listings
      .CountAsync(l => l.DeletedAt == null);
  }

  public async Task<int> GetDeletedListingsCount()
  {
    return await _dbContext.Listings
      .CountAsync(l => l.DeletedAt != null);
  }

  public IQueryable<Listing> GetActiveListings()
  {
    return _dbContext.Listings
      .Include(l => l.Publisher);
  }

  public async Task<Listing?> GetListing(Guid listingId)
  {
    return await _dbContext.Listings
      .Include(l => l.Publisher)
      .Where(l => l.Id == listingId)
      .FirstOrDefaultAsync();
  }

  public async Task DeleteListing(Listing listing)
  {
    listing.DeletedAt = DateTime.UtcNow;
    await _dbContext.SaveChangesAsync();
    _logger.LogInformation($"Deleted listing {listing.Id}");
  }

  public async Task RestoreListing(Listing listing)
  {
    listing.DeletedAt = null;
    await _dbContext.SaveChangesAsync();
    _logger.LogInformation($"Restored listing {listing.Id}");
  }

  public async Task UpdateListing(Listing listing, UpdateListingDto updateDto)
  {
    var config = new MapperConfiguration(cfg => cfg.CreateMap<UpdateListingDto, Listing>());
    IMapper mapper = config.CreateMapper();
    mapper.Map(updateDto, listing);
    listing.UpdatedAt = DateTime.UtcNow;
    await _dbContext.SaveChangesAsync();
  }

  public async Task CreateListing(CreateListingDto createDto, Guid publisherId)
  {
    var listing = new Listing();

    if (createDto.PreviewImage != null)
    {
      string b64 = Regex.Replace(
        createDto.PreviewImage.Base64Body,
        @"^(data:image\/[a-zA-Z]+;base64,|base64,)",
        string.Empty
      );

      byte[] bytes = Convert.FromBase64String(b64);
      string filename = $"{Guid.NewGuid()}.{Path.GetExtension(createDto.PreviewImage.FileName)[1..]}";
      listing.PreviewFileName = filename;

      _fileService.Create(filename, bytes);
    }

    var config = new MapperConfiguration(cfg => cfg.CreateMap<CreateListingDto, Listing>());
    IMapper mapper = config.CreateMapper();
    mapper.Map(createDto, listing);

    User publisher = (await _dbContext.Users.FindAsync(publisherId))!;
    listing.Publisher = publisher;
    listing.PublisherId = publisher.Id;

    await _dbContext.Listings.AddAsync(listing);
    await _dbContext.SaveChangesAsync();
  }
}
