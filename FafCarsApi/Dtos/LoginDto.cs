using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FafCarsApi.Dtos;

public class LoginDto {
  [DefaultValue("user")]
  [Length(1, 255)]
  public string Username { get; set; } = null!;

  [DefaultValue("user")]
  [Length(1, 255)]
  public string Password { get; set; } = null!;
}
