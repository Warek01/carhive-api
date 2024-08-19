using Api.Dtos.Response;
using Api.Models;

namespace Api.Services;

public class ListingMappingService(
  IConfiguration configuration
) {
  public ListingDto ListingToDto(Listing listing) {
    Listing? l = listing;

    var dto = new ListingDto {
      Id = l.Id,
      Description = l.Description,
      Clearance = l.Clearance,
      Color = l.Color,
      WheelSize = l.WheelSize,
      Drivetrain = l.Drivetrain,
      Horsepower = l.Horsepower,
      Mileage = l.Mileage,
      Price = l.Price,
      Status = l.Status,
      CarStatus = l.CarStatus,
      CityName = l.CityName,
      Views = l.Views,
      CreatedAt = l.CreatedAt,
      BodyStyle = l.BodyStyle,
      BrandName = l.BrandName,
      EngineVolume = l.EngineVolume,
      CountryCode = l.CountryCode,
      FuelType = l.FuelType,
      ModelName = l.ModelName,
      SellAddress = l.SellAddress,
      ProductionYear = l.ProductionYear,
      UpdatedAt = l.UpdatedAt,
      SoldAt = l.SoldAt,
      BlockedAt = l.BlockedAt,
      DeletedAt = l.DeletedAt,
    };

    var baseUri = new Uri(configuration["BaseUrl"]!);

    dto.ImagesUrls = l.Images
      .Select(fileName =>
        new Uri(baseUri, Path.Combine(StaticFileService.RequestPath, StaticFileService.UploadsPath, fileName))
      )
      .ToList();

    dto.Publisher = new ListingDto.PublisherDto {
      Id = l.PublisherId,
      Username = l.Publisher.Username,
      Picture = l.Publisher.Picture,
    };

    return dto;
  }

  public List<ListingDto> ListingsToDto(List<Listing> listings) {
    return listings.Select(ListingToDto).ToList();
  }
}
