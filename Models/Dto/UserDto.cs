using FafCarsApi.Models.Entities;

namespace FafCarsApi.Models.Dto;

public class UserDto
{
  public Guid Id { get; set; }
  public string Username { get; set; } = null!;

  public static UserDto CreateFromUser(User u)
  {
    return new UserDto
    {
      Id = u.Id,
      Username = u.Username
    };
  }
}
