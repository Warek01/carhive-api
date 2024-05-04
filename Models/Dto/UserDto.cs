namespace FafCarsApi.Models.Dto;

public class UserDto
{
  public Guid Id { get; set; }
  public string Username { get; set; } = null!;
  public ICollection<ListingDto>? Listings { get; set; }
}
