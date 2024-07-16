using System.ComponentModel.DataAnnotations;
using FafCarsApi.Enums;

namespace FafCarsApi.Dto;

public class UpdateUserDto {
  [Length(1, 255)]
  public string Username { get; set; } = null!;

  [Length(1, 255)]
  public string Email { get; set; } = null!;

  public UserRole Role { get; set; }
}
