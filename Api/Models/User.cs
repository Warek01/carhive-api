using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Api.Data;
using Api.Enums;

namespace Api.Models;

[Table("users")]
public class User {
  [Key]
  [Column("id")]
  public Guid Id { get; set; }

  [StringLength(255)]
  [Column("username")]
  public string Username { get; set; } = null!;

  [StringLength(255)]
  [Column("password")]
  public string Password { get; set; } = null!;

  [StringLength(255)]
  [Column("email")]
  public string Email { get; set; } = null!;

  [Column("roles")]
  public List<UserRole> Roles { get; set; } = [UserRole.User];

  public List<Listing> Listings { get; set; } = [];

  [Column("deleted_at", TypeName = CarHiveDbContext.TimestampNoTimezoneSql)]
  public DateTime? DeletedAt { get; set; }

  [Column("created_at", TypeName = CarHiveDbContext.TimestampNoTimezoneSql)]
  public DateTime CreatedAt { get; set; }

  [Column("updated_at", TypeName = CarHiveDbContext.TimestampNoTimezoneSql)]
  public DateTime UpdatedAt { get; set; }

  [StringLength(255)]
  [Column("phone_number")]
  public string? PhoneNumber { get; set; }

  [InverseProperty(nameof(Listing.UsersFavorites))]
  public List<Listing> Favorites { get; set; } = [];

  [InverseProperty(nameof(Report.User))]
  public List<Report> Reports { get; set; } = [];
}
