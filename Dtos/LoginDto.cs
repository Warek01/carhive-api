using System.ComponentModel;

namespace FafCarsApi.Dto;

public class LoginDto {
  [DefaultValue("user")]
  public string Username { get; set; } = null!;
  [DefaultValue("user")]
  public string Password { get; set; } = null!;
}
