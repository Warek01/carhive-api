using System.ComponentModel;
using FafCarsApi.Enums;

namespace FafCarsApi.Dtos.Request;

public class FavoriteListingActionDto {
  [DefaultValue(FavoriteListingAction.Add)]
  public FavoriteListingAction Type { get; set; }

  public Guid? ListingId { get; set; }
}
