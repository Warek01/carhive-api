using System.ComponentModel.DataAnnotations;
using FafCarsApi.Enums;

namespace FafCarsApi.Dtos;

public class UpdateListingDto {
  [Length(1, 255)]
  public string BrandName { get; set; } = null!;
  
  [Length(1, 255)]
  public string ModelName { get; set; } = null!;
  
  [Range(0, double.MaxValue)]
  public double Price { get; set; }
  
  [Length(1, 255)]
  public string Type { get; set; } = null!;
  
  [Range(0, int.MaxValue)]
  public int? Horsepower { get; set; }
  
  public EngineType EngineType { get; set; }
  
  [Range(0, double.MaxValue)]
  public double? EngineVolume { get; set; }
  
  public CarColor Color { get; set; }
  
  [Range(0, int.MaxValue)]
  public int? Clearance { get; set; }
  
  [Range(0, int.MaxValue)]
  public int? WheelSize { get; set; }
  
  [Range(0, int.MaxValue)]
  public int? Mileage { get; set; }
  
  [Range(0, int.MaxValue)]
  public int? ProductionYear { get; set; }
  
  public FileDto? PreviewFile { get; set; }
  
  public List<FileDto> ImagesFiles { get; set; } = [];
}
