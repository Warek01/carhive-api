namespace FafCarsApi.Models.Dto;

public class FileDto {
  public string? FileName { get; set; }
  public string Base64Body { get; set; } = null!;

  public void Deconstruct(out string? fileName, out string base64Body) {
    fileName = FileName;
    base64Body = Base64Body;
  }
}
