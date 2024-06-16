using System.ComponentModel.DataAnnotations;

namespace FafCarsApi.Models;

public class Country {
  [Key] [StringLength(2)] public string Code { get; set; } = null!;
  [StringLength(255)] public string Name { get; set; } = null!;

  public List<Listing> Listings { get; set; } = [];
  public List<Brand> Brands { get; set; } = [];
  public List<City> Cities { get; set; } = [];
}
