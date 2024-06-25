using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FafCarsApi.Models;

[Table("countries")]
public class Country {
  [Key]
  [StringLength(2)]
  [Column("code")]
  public string Code { get; set; } = null!;

  [StringLength(255)]
  [Column("name")]
  public string Name { get; set; } = null!;

  [InverseProperty(nameof(Listing.Country))]
  public List<Listing> Listings { get; set; } = [];

  [InverseProperty(nameof(Brand.Country))]
  public List<Brand> Brands { get; set; } = [];
}