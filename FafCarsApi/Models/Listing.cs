using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FafCarsApi.Models;

public class Listing {
  [Key] public Guid Id { get; set; }

  [StringLength(255)] public string BrandName { get; set; } = null!;

  [StringLength(255)] public string ModelName { get; set; } = null!;

  [Range(0, double.MaxValue)] public double Price { get; set; }

  [StringLength(255)] public string Type { get; set; } = null!;

  [Range(0, int.MaxValue)] public int? Horsepower { get; set; }

  [StringLength(255)] public string? EngineType { get; set; }

  public double? EngineVolume { get; set; }

  [StringLength(7)] public string? Color { get; set; } = null!;

  [Range(0, int.MaxValue)] public int? Clearance { get; set; }

  [Range(0, int.MaxValue)] public int? WheelSize { get; set; }

  [Range(0, int.MaxValue)] public int? Mileage { get; set; }

  [Range(0, int.MaxValue)] public int? Year { get; set; }

  [StringLength(255)] public string? Preview { get; set; }

  public List<string> Images { get; set; } = new();

  public Guid PublisherId { get; set; }

  [Column(TypeName = "TIMESTAMP(1) WITHOUT TIME ZONE")]
  public DateTime? DeletedAt { get; set; }

  [Column(TypeName = "TIMESTAMP(1) WITHOUT TIME ZONE")]
  public DateTime CreatedAt { get; set; }

  [Column(TypeName = "TIMESTAMP(1) WITHOUT TIME ZONE")]
  public DateTime UpdatedAt { get; set; }

  public User Publisher { get; set; } = null!;

  public List<User> UsersFavorites { get; set; } = [];
}
