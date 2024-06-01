using System.ComponentModel;

namespace FafCarsApi.Dto;

public class CreateListingDto {
  [DefaultValue("Toyota")]
  public string BrandName { get; set; } = null!;
  [DefaultValue("Camry")]
  public string ModelName { get; set; } = null!;
  [DefaultValue(12500)]
  public double Price { get; set; }
  [DefaultValue("Sedan")]
  public string Type { get; set; } = null!;
  [DefaultValue(220)]
  public int? Horsepower { get; set; }
  [DefaultValue("Hybrid")]
  public string? EngineType { get; set; }
  [DefaultValue(3.5)]
  public double? EngineVolume { get; set; }
  [DefaultValue("#000000")]
  public string? Color { get; set; }
  [DefaultValue(20)]
  public int? Clearance { get; set; }
  [DefaultValue(20)]
  public int? WheelSize { get; set; }
  [DefaultValue(80000)]
  public int? Mileage { get; set; }
  [DefaultValue(2019)]
  public int? Year { get; set; }
  public FileDto? Preview { get; set; }
  public List<FileDto> Images { get; set; } = new();
}
