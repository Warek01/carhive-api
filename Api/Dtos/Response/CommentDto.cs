namespace Api.Dtos.Response;

public class CommentDto {
  public Guid Id { get; set; }
  public Guid ListingId { get; set; }
  public string Content { get; set; } = null!;
  public DateTime CreatedAt { get; set; }
}
