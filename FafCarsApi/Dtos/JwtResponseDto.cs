namespace FafCarsApi.Dtos;

public class JwtResponseDto {
  public string Token { get; set; } = null!;

  public string RefreshToken { get; set; } = null!;
}
