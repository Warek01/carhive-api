using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FafCarsApi.Models;

public class Brand {
  [Key]
  [StringLength(255)] public string Name { get; set; } = null!;

  public IList<Listing> Listings { get; set; } = [];
}
