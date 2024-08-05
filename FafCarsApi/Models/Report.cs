using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FafCarsApi.Data;
using FafCarsApi.Enums;

namespace FafCarsApi.Models;

[Table("reports")]
public class Report {
  [Key]
  [Column("id")]
  public Guid Id { get; set; }
  
  [Column("type")]
  public ReportType Type { get; set; }
  
  [Column("user_id")]
  public Guid UserId { get; set; }

  [ForeignKey(nameof(UserId))]
  [InverseProperty(nameof(User.Reports))]
  public User User { get; set; } = null!;
  
  [Column("reason")]
  [StringLength(500)]
  public string? Reason { get; set; }
  
  [Column("created_at", TypeName = FafCarsDbContext.TimestampNoTimezoneSql)]
  public DateTime CreatedAt { get; set; }
}
