using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FafCarsApi.Enums;

namespace FafCarsApi.Models;

public class Listing {
  [Key] public Guid Id { get; set; }

  [StringLength(255)] public string BrandName { get; set; } = null!;
  public Brand Brand { get; set; } = null!;

  [StringLength(255)] public string? ModelName { get; set; }
  public Model? Model { get; set; }

  [Range(0, double.MaxValue)] public double Price { get; set; }

  public BodyStyle BodyStyle { get; set; }

  [Range(0, int.MaxValue)] public int? Horsepower { get; set; }

  public EngineType EngineType { get; set; }

  public double? EngineVolume { get; set; }

  public CarColor? Color { get; set; }

  [Range(0, int.MaxValue)] public int? Clearance { get; set; }

  [Range(0, int.MaxValue)] public int? WheelSize { get; set; }

  [Range(0, int.MaxValue)] public int? Mileage { get; set; }

  [Range(0, int.MaxValue)] public int? Year { get; set; }

  [StringLength(255)] public string? Preview { get; set; }

  public List<string> Images { get; set; } = [];

  public Guid PublisherId { get; set; }

  [Column(TypeName = "TIMESTAMP(1) WITHOUT TIME ZONE")]
  public DateTime? DeletedAt { get; set; }

  [Column(TypeName = "TIMESTAMP(1) WITHOUT TIME ZONE")]
  public DateTime CreatedAt { get; set; }

  [Column(TypeName = "TIMESTAMP(1) WITHOUT TIME ZONE")]
  public DateTime UpdatedAt { get; set; }

  public User Publisher { get; set; } = null!;

  public List<User> UsersFavorites { get; set; } = [];

  [StringLength(2)] public string CountryCode { get; set; } = null!;

  public Country Country { get; set; } = null!;

  [StringLength(255)] public string? Address { get; set; }
}
