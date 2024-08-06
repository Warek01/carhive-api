using System.ComponentModel.DataAnnotations;

namespace Api.Dtos.Response;

public class CountryDto {
  public string Name { get; set; } = null!;
  public string Code { get; set; } = null!;
}
