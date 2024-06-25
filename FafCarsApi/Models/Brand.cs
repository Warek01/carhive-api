using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FafCarsApi.Models;

[Table("brands")]
public class Brand {
  [Key]
  [StringLength(255)]
  [Column("name")]
  public string Name { get; set; } = null!;

  [StringLength(2)]
  [Column("country_code")]
  public string CountryCode { get; set; } = null!;

  [ForeignKey(nameof(CountryCode))]
  [InverseProperty(nameof(Country.Brands))]
  public Country? Country { get; set; }

  [InverseProperty(nameof(Listing.Brand))]
  public List<Listing> Listings { get; set; } = [];

  [InverseProperty(nameof(Model.Brand))]
  public List<Model> Models { get; set; } = [];
}