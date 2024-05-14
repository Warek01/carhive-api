namespace FafCarsApi.Models.Dto;

public class CreateListingDto
{
  public string BrandName { get; set; } = null!;
  public string ModelName { get; set; } = null!;
  public double Price { get; set; }
  public string Type { get; set; } = null!;
  public int? Horsepower { get; set; }
  public string? EngineType { get; set; }
  public double? EngineVolume { get; set; }
  public string? Color { get; set; }
  public int? Clearance { get; set; }
  public int? WheelSize { get; set; }
  public int? Mileage { get; set; }
  public int? Year { get; set; }
  public FileDto? PreviewImage { get; set; }
}
