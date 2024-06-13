using System.ComponentModel;
using FafCarsApi.Enums;

namespace FafCarsApi.Dtos;

public class CreateUserDto {
  [DefaultValue("warek")]
  public string Username { get; set; } = null!;
  [DefaultValue("warek")]
  public string Password { get; set; } = null!;
  [DefaultValue("warek@gmail.com")]
  public string Email { get; set; } = null!;
  public List<UserRole> Roles { get; set; } = null!;
}
