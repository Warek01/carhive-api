using System.ComponentModel.DataAnnotations;
using Api.Enums;
using Api.Helpers;

namespace Api.Dtos.Request;

public class UpdateUserDto {
  [Length(1, 255)]
  public string? Username { get; set; }

  public bool UpdateUsername { get; set; } = false;

  [Length(1, 255)]
  [RegularExpression(RegexStrings.Email)]
  public string? Email { get; set; }

  public bool UpdateEmail { get; set; } = false;

  [MinLength(1)]
  public List<UserRole>? Roles { get; set; }

  public bool UpdateRoles { get; set; } = false;

  public UserStatus? Status { get; set; }

  public bool UpdateStatus { get; set; } = false;

  public string? FirstName { get; set; }

  public bool UpdateFirstName { get; set; } = false;

  public string? LastName { get; set; }

  public bool UpdateLastName { get; set; } = false;

  public Uri? Picture { get; set; }

  public bool UpdatePicture { get; set; } = false;
}
