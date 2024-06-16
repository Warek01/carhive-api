using System.ComponentModel.DataAnnotations;

namespace FafCarsApi.Models;

public class Brand {
  [Key] [StringLength(255)] public string Name { get; set; } = null!;

  [StringLength(2)] public string CountryCode { get; set; } = null!;

  public Country? Country { get; set; }

  public List<Listing> Listings { get; set; } = [];

  public List<Model> Models { get; set; } = [];
}
