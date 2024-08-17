namespace Api.Queries;

public class CommentQuery : PaginationQuery {
  public Guid? ListingId { get; set; }
  public Guid? UserId { get; set; }
}
