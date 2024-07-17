using FafCarsApi.Enums;

namespace FafCarsApi.Dtos.Response;

public class ListingDto {
  public Guid Id { get; set; }
  public string BrandName { get; set; } = null!;
  public string ModelName { get; set; } = null!;
  public DateTime UpdatedAt { get; set; }
  public DateTime CreatedAt { get; set; }
  public double? Price { get; set; }
  public CarBodyStyle? BodyStyle { get; set; }
  public int? Horsepower { get; set; }
  public CarFuelType? FuelType { get; set; }
  public double? EngineVolume { get; set; }
  public CarColor? Color { get; set; }
  public int? Clearance { get; set; }
  public int? WheelSize { get; set; }
  public int? Mileage { get; set; }
  public int? ProductionYear { get; set; }
  public List<string> ImagesUrls { get; set; } = [];
  public DateTime? DeletedAt { get; set; }
  public UserDto? Publisher { get; set; }
  public string? CountryCode { get; set; } = null!;
  public string? City { get; set; }
  public string? SellAddress { get; set; }
  public bool? IsFavorite { get; set; }
  public string? Description { get; set; }
  public CarStatus? CarStatus { get; set; }
  public ListingStatus Status { get; set; }
  public DateTime? BlockedAt { get; set; }
  public DateTime? SoldAt { get; set; }
}
