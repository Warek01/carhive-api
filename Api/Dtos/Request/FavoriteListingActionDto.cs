using System.ComponentModel;
using Api.Enums;

namespace Api.Dtos.Request;

public class FavoriteListingActionDto {
  [DefaultValue(FavoriteListingAction.Add)]
  public FavoriteListingAction Type { get; set; }

  public Guid? ListingId { get; set; }
}
