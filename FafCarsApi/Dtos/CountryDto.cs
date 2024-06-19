using System.ComponentModel.DataAnnotations;

namespace FafCarsApi.Dtos;

public class CountryDto {
  [Length(1, 255)]
  public string Name { get; set; } = null!;
  
  [Length(2, 2)]
  public string Code { get; set; } = null!;
}
