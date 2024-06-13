namespace FafCarsApi.Dtos;

public class PaginatedResultDto<T> {
  public ICollection<T> Items { get; set; } = null!;
  public int TotalItems { get; set; }
}
