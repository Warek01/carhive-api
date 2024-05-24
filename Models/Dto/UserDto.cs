using AutoMapper;
using FafCarsApi.Models.Entities;

namespace FafCarsApi.Models.Dto;

public class UserDto {
  public Guid Id { get; set; }
  public string Username { get; set; } = null!;
  public string Email { get; set; } = null!;
  public DateTime CreatedAt { get; set; }

  public static UserDto FromUser(User user) {
    var config = new MapperConfiguration(cfg => cfg.CreateMap<User, UserDto>());
    var mapper = config.CreateMapper();
    var dto = new UserDto();

    mapper.Map(user, dto);

    return dto;
  }
}
