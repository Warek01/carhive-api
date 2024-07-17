using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using FafCarsApi.Enums;

namespace FafCarsApi.Dtos.Request;

/// <summary>
/// Dto for creation of a user by admin
/// </summary>
public class CreateUserDto {
  [DefaultValue("warek")]
  [Length(1, 255)]
  public string Username { get; set; } = null!;

  [DefaultValue("warek")]
  [Length(1, 255)]
  public string Password { get; set; } = null!;

  [DefaultValue("warek@gmail.com")]
  [Length(1, 255)]
  public string Email { get; set; } = null!;

  public List<UserRole> Roles { get; set; } = null!;
}
