using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FafCarsApi.Enums;

namespace FafCarsApi.Models;

public class User {
  public static readonly User[] InitialData = [
    new User {
      Id = Guid.Parse("CDB7604F-DDDA-439C-8139-BFFDA01A8580"),
      Username = "admin",
      Password = BCrypt.Net.BCrypt.EnhancedHashPassword(
        "admin",
        13
      ),
      Email = "admin@gmail.com",
      Roles = new[] {
        UserRole.Admin,
        UserRole.ListingCreator,
      },
      CreatedAt = DateTime.Now,
      UpdatedAt = DateTime.Now,
      DeletedAt = null,
      PhoneNumber = "+37378000111",
      Listings = new List<Listing>()
    },
    new User {
      Id = Guid.Parse("5DF812C8-D8BE-4A9F-92F3-0CC5B3B78A1D"),
      Username = "user",
      Password = BCrypt.Net.BCrypt.EnhancedHashPassword(
        "user",
        13
      ),
      Email = "user@gmail.com",
      Roles = new[] {
        UserRole.Admin
      },
      CreatedAt = DateTime.Now,
      UpdatedAt = DateTime.Now,
      DeletedAt = null,
      PhoneNumber = "+37378111222",
      Listings = new List<Listing>()
    }
  ];

  [Key] public Guid Id { get; set; }

  [StringLength(255)] public string Username { get; set; } = null!;

  [StringLength(255)] public string Password { get; set; } = null!;

  [StringLength(255)] public string Email { get; set; } = null!;

  public IList<UserRole> Roles { get; set; } = [];

  public IList<Listing> Listings { get; set; } = [];

  [Column(TypeName = "TIMESTAMP(1) WITHOUT TIME ZONE")]
  public DateTime? DeletedAt { get; set; }

  [Column(TypeName = "TIMESTAMP(1) WITHOUT TIME ZONE")]
  public DateTime CreatedAt { get; set; }

  [Column(TypeName = "TIMESTAMP(1) WITHOUT TIME ZONE")]
  public DateTime UpdatedAt { get; set; }

  [StringLength(255)] public string? PhoneNumber { get; set; }

  public IList<Listing> Favorites { get; set; } = [];
}
