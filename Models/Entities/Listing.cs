using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FafCarsApi.Models.Entities;

[Table("listings")]
public class Listing {
  [Key] [Column("id")] public Guid Id { get; set; }

  [Column("brand")] [StringLength(255)] public string BrandName { get; set; } = null!;

  [Column("model")] [StringLength(255)] public string ModelName { get; set; } = null!;

  [Column("price")]
  [Range(0, double.MaxValue)]
  public double Price { get; set; }

  [Column("type")] [StringLength(255)] public string Type { get; set; } = null!;

  [Column("horsepower")]
  [Range(0, int.MaxValue)]
  public int? Horsepower { get; set; }

  [Column("engine_type")]
  [StringLength(255)]
  public string? EngineType { get; set; }

  [Column("engine_volume")] public double? EngineVolume { get; set; }

  [Column("color")] [StringLength(7)] public string? Color { get; set; } = null!;

  [Column("clearance")]
  [Range(0, int.MaxValue)]
  public int? Clearance { get; set; }

  [Column("wheel_size")]
  [Range(0, int.MaxValue)]
  public int? WheelSize { get; set; }

  [Column("mileage")]
  [Range(0, int.MaxValue)]
  public int? Mileage { get; set; }

  [Column("year")]
  [Range(0, int.MaxValue)]
  public int? Year { get; set; }

  [Column("preview")]
  [StringLength(255)]
  public string? Preview { get; set; }

  [Column("images")] public List<string> Images { get; set; } = new();

  [Column("publisher_id")] public Guid PublisherId { get; set; }

  [Column("deleted_at", TypeName = "TIMESTAMP(1) WITHOUT TIME ZONE")]
  public DateTime? DeletedAt { get; set; }

  [Column("created_at", TypeName = "TIMESTAMP(1) WITHOUT TIME ZONE")]
  public DateTime CreatedAt { get; set; }

  [Column("updated_at", TypeName = "TIMESTAMP(1) WITHOUT TIME ZONE")]
  public DateTime UpdatedAt { get; set; }

  public User Publisher { get; set; } = null!;
}
