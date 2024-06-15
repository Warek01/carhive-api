using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FafCarsApi.Enums;

namespace FafCarsApi.Models;

public class User {
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
