using Api.Enums;

namespace Api.Dtos.Response;

/// <summary>
///  Dto of user account for non admins
/// </summary>
public class UserDto {
  public Guid Id { get; set; }

  public string Username { get; set; } = null!;

  public string Email { get; set; } = null!;

  public string? PhoneNumber { get; set; }

  public string? Picture { get; set; }

  public string? FirstName { get; set; }

  public string? LastName { get; set; }

  public OauthIdentityProvider? OauthIdentityProvider { get; set; }
}
