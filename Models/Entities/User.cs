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
  
  [Column("phone_number")]
  [StringLength(255)]
  public string? PhoneNumber { get; set; }

  public static readonly User[] InitialData =
  {
    new User
    {
      Id = Guid.Parse("CDB7604F-DDDA-439C-8139-BFFDA01A8580"),
      Username = "admin",
      Password = BCrypt.Net.BCrypt.EnhancedHashPassword(
        "admin",
        13
      ),
      Email = "alexandrudobrojan@gmail.com",
      Roles = new UserRole[]
      {
        UserRole.Admin,
      },
      CreatedAt = DateTime.Now,
      UpdatedAt = DateTime.Now,
      DeletedAt = null,
      PhoneNumber = "+37378009584",
      Listings = new List<Listing>(),
    },
  };
}
