using System.ComponentModel.DataAnnotations;

namespace FafCarsApi.Models;

public class Model {
  [Key] [StringLength(255)] public string Name { get; set; } = null!;

  public List<Brand> Brands { get; set; } = [];
  public List<Listing> Listings { get; set; } = [];
}
