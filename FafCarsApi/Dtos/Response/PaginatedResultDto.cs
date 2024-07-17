namespace FafCarsApi.Dtos.Response;

public class PaginatedResultDto<T> {
  public List<T> Items { get; set; } = null!;

  public int TotalItems { get; set; }
}
