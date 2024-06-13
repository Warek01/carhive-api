namespace FafCarsApi.Dtos;

public class OperationResultDto {
  public bool Success { get; set; } = true;
  public string? Error { get; set; } = null;
}
