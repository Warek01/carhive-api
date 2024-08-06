using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Api.Models;

[Table("listing_user_favorite")]
[PrimaryKey(nameof(UserId), nameof(ListingId))]
public class ListingUserFavorite {
  [Key]
  [Column("user_id")]
  public Guid UserId { get; set; }

  [Key]
  [Column("listing_id")]
  public Guid ListingId { get; set; }

  [ForeignKey(nameof(UserId))]
  public User User { get; set; } = null!;

  [ForeignKey(nameof(ListingId))]
  public Listing Listing { get; set; } = null!;
}