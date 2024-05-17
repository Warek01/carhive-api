using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FafCarsApi.Models.Entities;

[Table("users")]
public class User
{
  [Key] [Column("id")] public Guid Id { get; set; }

  [Column("username")]
  [StringLength(255)]
  public string Username { get; set; } = null!;

  [Column("password")]
  [StringLength(60)]
  public string Password { get; set; } = null!;

  [Column("email")] [StringLength(255)] public string Email { get; set; } = null!;

  [Column("roles")] public IList<UserRole> Roles { get; set; } = null!;

  public ICollection<Listing> Listings { get; set; } = null!;
  
  [Column("deleted_at", TypeName = "TIMESTAMP(0) WITHOUT TIME ZONE")]
  public DateTime? DeletedAt { get; set; }

  [Column("created_at", TypeName = "TIMESTAMP(0) WITHOUT TIME ZONE")]
  public DateTime CreatedAt { get; set; }

  [Column("updated_at", TypeName = "TIMESTAMP(0) WITHOUT TIME ZONE")]
  public DateTime UpdatedAt { get; set; }

  public static readonly User[] MockUsers =
  {
    new User
    {
      Id = Guid.NewGuid(),
      Username = "warek",
      Password = BCrypt.Net.BCrypt.EnhancedHashPassword(
        "warek",
        13
      ),
      Email = "warek@gmail.com",
      Roles = new UserRole[]
      {
        UserRole.Admin
      },
      CreatedAt = DateTime.Now,
      UpdatedAt = DateTime.Now,
    },
    new User
    {
      Id = Guid.NewGuid(),
      Username = "denis",
      Password = BCrypt.Net.BCrypt.EnhancedHashPassword(
        "denis",
        13
      ),
      Email = "denis@gmail.com",
      Roles = new UserRole[]
      {
        UserRole.User, UserRole.CreateListing
      },
      CreatedAt = DateTime.Now,
      UpdatedAt = DateTime.Now,
    },
  };
}
