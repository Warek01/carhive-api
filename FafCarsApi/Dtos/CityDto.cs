using System.ComponentModel.DataAnnotations;

namespace FafCarsApi.Dto;

public class CityDto {
  [Length(1, 255)]
  public string Name { get; set; } = null!;
}