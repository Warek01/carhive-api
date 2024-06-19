using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FafCarsApi.Models;

[PrimaryKey(nameof(Name), nameof(BrandName))]
[Table("models")]
public class Model {
  [Key] 
  [StringLength(255)] 
  [Column("name")]
  public string Name { get; set; } = null!;

  [Key]
  [StringLength(255)]
  [Column("brand_name")]
  public string BrandName { get; set; } = null!;

  [ForeignKey(nameof(BrandName))]
  [InverseProperty(nameof(Brand.Models))]
  public Brand Brand { get; set; } = null!;
  
  [InverseProperty(nameof(Listing.Model))]
  public List<Listing> Listings { get; set; } = [];
}
