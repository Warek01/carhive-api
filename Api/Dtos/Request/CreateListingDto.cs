using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Api.Enums;

namespace Api.Dtos.Request;

public class CreateListingDto {
  [DefaultValue("Toyota")]
  public string BrandName { get; set; } = null!;

  [DefaultValue("Camry")]
  public string ModelName { get; set; } = null!;
  
  [StringLength(255)]
  [DefaultValue("Belin")]
  public string CityName { get; set; } = null!;
  
  public List<IFormFile> Images { get; set; } = [];

  [Length(2, 2)]
  [DefaultValue("DE")]
  public string CountryCode { get; set; } = null!;
  
  [DefaultValue(CarDrivetrain.RearWheelDrive)]
  public CarDrivetrain? Drivetrain { get; set; }
  
  [DefaultValue(CarTransmission.Automatic)]
  public CarTransmission? Transmission { get; set; }

  [DefaultValue(12500)]
  [Range(0, double.MaxValue)]
  public double? Price { get; set; }

  [DefaultValue(CarBodyStyle.Sedan)]
  public CarBodyStyle? BodyStyle { get; set; }
  
  [DefaultValue(220)]
  [Range(0, int.MaxValue)]
  public int? Horsepower { get; set; }

  [DefaultValue(CarFuelType.Hybrid)]
  public CarFuelType? FuelType { get; set; }
  
  [DefaultValue(3.5)]
  [Range(0, double.MaxValue)]
  public double? EngineVolume { get; set; }
    
  [DefaultValue(CarColor.Black)]
  public CarColor? Color { get; set; }

  [DefaultValue(20)]
  [Range(0, int.MaxValue)]
  public int? Clearance { get; set; }

  [DefaultValue(20)]
  [Range(0, int.MaxValue)]
  public int? WheelSize { get; set; }

  [DefaultValue(80000)]
  [Range(0, int.MaxValue)]
  public int? Mileage { get; set; }

  [DefaultValue(2019)]
  [Range(0, int.MaxValue)]
  public int? ProductionYear { get; set; }

  [StringLength(255)]
  [DefaultValue("789 Oak Rd, Berlin, DE")]
  public string? SellAddress { get; set; }
  
  [DefaultValue(Enums.CarStatus.Used)]
  public CarStatus? CarStatus { get; set; }
  
  [StringLength(5000)]
  [DefaultValue("Random Text")]
  public string? Description { get; set; }
}
