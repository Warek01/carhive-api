using System.ComponentModel.DataAnnotations;

namespace Api.Dtos.Request;

public class CreateCommentDto {
  [StringLength(5000)]
  public string Content { get; set; } = null!;

  public Guid ListingId { get; set; }
}
