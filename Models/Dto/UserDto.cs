using FafCarsApi.Models.Entities;

namespace FafCarsApi.Models.Dto;

public class UserDto
{
  public Guid Id { get; set; }
  public string Username { get; set; } = null!;
  public string Email { get; set; } = null!;
  public IList<UserRole> Roles { get; set; } = null!;
  public DateTime? DeletedAt { get; set; }
  public DateTime CreatedAt { get; set; }
  public DateTime UpdatedAt { get; set; }

  public static UserDto FromUser(User u)
  {
    return new UserDto
    {
      Id = u.Id,
      Username = u.Username,
      Email = u.Email,
      Roles = u.Roles,
      DeletedAt = u.DeletedAt,
      CreatedAt = u.CreatedAt,
      UpdatedAt = u.UpdatedAt,
    };
  }
}
