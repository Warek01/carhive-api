using System.ComponentModel;

namespace FafCarsApi.Dtos;

public class LoginDto {
  [DefaultValue("user")]
  public string Username { get; set; } = null!;
  [DefaultValue("user")]
  public string Password { get; set; } = null!;
}
