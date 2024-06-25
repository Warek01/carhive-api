using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FafCarsApi.Dto;

public class RegisterDto {
  [DefaultValue("new_user")]
  [StringLength(255)]
  public string Username { get; set; } = null!;

  [DefaultValue("password")]
  [StringLength(255)]
  public string Password { get; set; } = null!;

  [DefaultValue("email@gmail.com")]
  [StringLength(255)]
  public string Email { get; set; } = null!;
}