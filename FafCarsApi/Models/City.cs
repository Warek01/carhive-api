using System.ComponentModel.DataAnnotations;

namespace FafCarsApi.Models;

public class City {
  [Key] [StringLength(255)] public string Name { get; set; } = null!;
  [StringLength(2)] public string CountryCode { get; set; } = null!;

  public List<Listing> Listings { get; set; } = [];
  public Country Country { get; set; } = null!;
}
