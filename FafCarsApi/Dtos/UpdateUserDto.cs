using System.ComponentModel.DataAnnotations;
using FafCarsApi.Enums;
using FafCarsApi.Models;

namespace FafCarsApi.Dto;

public class UpdateUserDto {
  [MinLength(1)] public string Username { get; set; } = null!;
  [MinLength(1)] public string Email { get; set; } = null!;
  public IList<UserRole> Roles { get; set; } = null!;
}
