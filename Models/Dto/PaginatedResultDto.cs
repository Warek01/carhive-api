namespace FafCarsApi.Models.Dto;

public class PaginatedResultDto<T>
{
  public ICollection<T> Items { get; set; } = null!;
  public int Page { get; set; }
  public int PageSize { get; set; }
  public int TotalItems { get; set; }
  public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);
}
