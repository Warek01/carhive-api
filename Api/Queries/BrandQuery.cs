using System.ComponentModel.DataAnnotations;

namespace Api.Queries;

public class BrandQuery {
  [RegularExpression("^[a-zA-Z]{2}$")]
  public string? CountryCode { get; set; }

  [StringLength(255)]
  public string? Search { get; set; }
}
