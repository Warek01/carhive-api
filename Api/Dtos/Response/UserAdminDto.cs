using Api.Enums;

namespace Api.Dtos.Response;

/// <summary>
/// Dto of user accounts for admins
/// </summary>
public class UserAdminDto {
  public Guid Id { get; set; }

  public string Username { get; set; } = null!;

  public string? Email { get; set; }

  public string? PhoneNumber { get; set; }

  public List<UserRole> Roles { get; set; } = [];

  public DateTime CreatedAt { get; set; }
}
