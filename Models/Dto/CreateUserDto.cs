namespace FafCarsApi.Models.Dto;

public class CreateUserDto {
  public string Username { get; set; } = null!;
  public string Password { get; set; } = null!;
  public string Email { get; set; } = null!;
  public IList<UserRole> Roles { get; set; } = null!;
}