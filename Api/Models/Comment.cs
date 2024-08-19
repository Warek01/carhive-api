using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Api.Data;

namespace Api.Models;

[Table("comments")]
public class Comment {
  [Column("id")]
  [Key]
  public Guid Id { get; set; }

  [Column("content")]
  [StringLength(5000)]
  public string Content { get; set; } = null!;

  [Column("created_at", TypeName = CarHiveDbContext.TimestampNoTimezoneSql)]
  public DateTime CreatedAt { get; set; }

  [Column("user_id")]
  public Guid UserId { get; set; }

  [InverseProperty(nameof(User.Comments))]
  [ForeignKey(nameof(UserId))]
  public User User { get; set; } = null!;

  [Column("listing_id")]
  public Guid ListingId { get; set; }

  [InverseProperty(nameof(Listing.Comments))]
  [ForeignKey(nameof(ListingId))]
  public Listing Listing { get; set; } = null!;
}
