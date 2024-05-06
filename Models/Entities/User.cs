using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FafCarsApi.Models.Entities;

[Table("users")]
public class User
{
  [Key] 
  [Column("id")]
  public Guid Id { get; set; }

  [Column("username")] 
  [StringLength(255)]
  public string Username { get; set; } = null!;
  
  // TODO: encrypt
  [Column("password")]
  [StringLength(255)]
  public string Password { get; set; } = null!;
  
  public ICollection<Listing> Listings { get; set; } = null!;

  public static readonly User[] MockUsers =
  {
    new User
    {
      Id = Guid.NewGuid(),
      Username = "warek",
      Password = "warek"
    },
    new User
    {
      Id = Guid.NewGuid(),
      Username = "denis",
      Password = "denis"
    },
    new User
    {
      Id = Guid.NewGuid(),
      Username = "alex",
      Password = "alex"
    },
    new User
    {
      Id = Guid.NewGuid(),
      Username = "test",
      Password = "test"
    },
    new User
    {
      Id = Guid.NewGuid(),
      Username = "user",
      Password = "password"
    },
  };
}
