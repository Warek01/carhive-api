using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Api.Helpers;

namespace Api.Dtos.Request;

public class RegisterDto {
  [DefaultValue("new_user")]
  [StringLength(255)]
  public string Username { get; set; } = null!;

  [DefaultValue("password")]
  [StringLength(255)]
  public string Password { get; set; } = null!;

  [DefaultValue("email@gmail.com")]
  [StringLength(255)]
  [RegularExpression(RegexStrings.Email)]
  public string Email { get; set; } = null!;
}
