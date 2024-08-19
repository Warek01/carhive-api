using Api.Enums;

namespace Api.Dtos.Response;

/// <summary>
/// Dto of user accounts for admins
/// </summary>
public class UserAdminDto : UserDto {
  public List<UserRole> Roles { get; set; } = [];
  public DateTime CreatedAt { get; set; }
}
