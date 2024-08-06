using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Api.Models;

[Table("cities")]
[PrimaryKey(nameof(Name), nameof(CountryCode))]
public class City {
  [Key]
  [Column("name")]
  [StringLength(255)]
  public string Name { get; set; } = null!;

  [Key]
  [ForeignKey(nameof(CountryCode))]
  [Column("country")]
  [StringLength(2)]
  public string CountryCode { get; set; } = null!;

  [InverseProperty(nameof(Country.Cities))]
  public Country Country { get; set; } = null!;

  [InverseProperty(nameof(Listing.City))]
  public List<Listing> Listings { get; set; } = [];
}
