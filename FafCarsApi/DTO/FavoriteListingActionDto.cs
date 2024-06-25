using System.ComponentModel;
using FafCarsApi.Enums;

namespace FafCarsApi.Dto;

public class FavoriteListingActionDto {
  [DefaultValue(FavoriteListingAction.Add)]
  public FavoriteListingAction Type { get; set; }

  public Guid? ListingId { get; set; }
}