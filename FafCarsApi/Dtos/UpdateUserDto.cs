using System.ComponentModel.DataAnnotations;
using FafCarsApi.Enums;

namespace FafCarsApi.Dtos;

public class UpdateUserDto {
  [MinLength(1)] public string Username { get; set; } = null!;
  [MinLength(1)] public string Email { get; set; } = null!;
  public List<UserRole> Roles { get; set; } = null!;
}
