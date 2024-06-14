using FafCarsApi.Enums;

namespace FafCarsApi.Dtos;

public class FavoriteListingActionDto {
  public FavoriteListingAction Type { get; set; }
  public Guid? ListingId { get; set; }
}
