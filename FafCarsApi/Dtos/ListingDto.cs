using FafCarsApi.Enums;

namespace FafCarsApi.Dtos;

public class ListingDto {
  public Guid Id { get; set; }
  public string BrandName { get; set; } = null!;
  public string ModelName { get; set; } = null!;
  public double Price { get; set; }
  public BodyStyle BodyStyle { get; set; }
  public int? Horsepower { get; set; }
  public EngineType EngineType { get; set; }
  public double? EngineVolume { get; set; }
  public CarColor Color { get; set; }
  public int? Clearance { get; set; }
  public int? WheelSize { get; set; }
  public int? Mileage { get; set; }
  public int? ProductionYear { get; set; }
  public DateTime UpdatedAt { get; set; }
  public DateTime CreatedAt { get; set; }
  public UserDto? Publisher { get; set; }
  public string? PreviewUrl { get; set; }
  public List<string> ImagesUrls { get; set; } = [];
  public string CountryCode { get; set; } = null!;
  public string? SellAddress { get; set; }
  public bool? IsFavorite { get; set; }
}
