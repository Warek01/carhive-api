using System.ComponentModel.DataAnnotations;

namespace Api.Queries;

public class BrandQuery {
  [Length(2, 2)]
  public string? CountryCode { get; set; }
  
  [StringLength(255)]
  public string? Search { get; set; }
}
