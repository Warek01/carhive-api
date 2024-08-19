using System.ComponentModel.DataAnnotations;
using Api.Helpers;

namespace Api.Queries;

public class BrandQuery {
  [RegularExpression(RegexStrings.CountryCode)]
  public string? CountryCode { get; set; }

  [StringLength(255)]
  public string? Search { get; set; }
}
