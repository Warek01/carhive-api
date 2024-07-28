using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FafCarsApi.Data;
using FafCarsApi.Enums;

namespace FafCarsApi.Models;

[Table("listing_activity")]
public class ListingActivity {
  [Column("id")]
  [Key]
  public Guid Id { get; set; }
  
  [Column("type")]
  public ListingAction Type { get; set; }
  
  [Column("user_id")]
  public Guid? UserId { get; set; }
  
  [ForeignKey(nameof(UserId))]
  public User? User { get; set; }
  
  [Column("listing_id")]
  public Guid ListingId { get; set; }

  [ForeignKey(nameof(ListingId))]
  public Listing Listing { get; set; } = null!;
  
  [Column("timestamp", TypeName = FafCarsDbContext.TimestampNoTimezoneSql)]
  public DateTime Timestamp { get; set; }
}
