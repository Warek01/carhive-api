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

  public ICollection<Listing> Listings { get; set; } = null!;

  public static readonly User[] MockUsers =
  {
    new User
    {
      Id = Guid.NewGuid(),
      Username = "warek",
      Password = BCrypt.Net.BCrypt.EnhancedHashPassword(
        "warek",
        10
      ),
      Email = "warek@gmail.com",
    },
    new User
    {
      Id = Guid.NewGuid(),
      Username = "denis",
      Password = BCrypt.Net.BCrypt.EnhancedHashPassword(
        "denis",
        10
      ),
      Email = "denis@gmail.com",
    },
    new User
    {
      Id = Guid.NewGuid(),
      Username = "alex",
      Password = BCrypt.Net.BCrypt.EnhancedHashPassword(
        "alex",
        10
      ),
      Email = "alex@gmail.com",
    },
    new User
    {
      Id = Guid.NewGuid(),
      Username = "test",
      Password = BCrypt.Net.BCrypt.EnhancedHashPassword(
        "test",
        10
      ),
      Email = "test@gmail.com",
    },
    new User
    {
      Id = Guid.NewGuid(),
      Username = "user",
      Password = BCrypt.Net.BCrypt.EnhancedHashPassword(
        "password",
        10
      ),
      Email = "user@gmail.com",
    },
  };
}
