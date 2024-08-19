using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Api.Data;

namespace Api.Models;

[Table("likes")]
public class Like {
  [Column("entity_id")]
  [Key]
  public Guid EntityId { get; set; }

  [Column("user_id")]
  public Guid UserId { get; set; }

  [InverseProperty(nameof(User.Likes))]
  [ForeignKey(nameof(UserId))]
  public User User { get; set; } = null!;

  [Column("liked_at", TypeName = CarHiveDbContext.TimestampNoTimezoneSql)]
  public DateTime LikedAt { get; set; }
}
