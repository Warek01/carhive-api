using Api.Data;
using Api.Dtos.Request;
using Api.Dtos.Response;
using Api.Models;
using Api.Queries;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Services;

public class CommentService(
  CarHiveDbContext db,
  IMapper mapper
) {
  public async Task CreateComment(CreateCommentDto dto, Guid userId) {
    var comment = new Comment {
      Content = dto.Content,
      UserId = userId,
      ListingId = dto.ListingId,
    };

    await db.Comments.AddAsync(comment);
    await db.SaveChangesAsync();
  }

  public async Task<ActionResult<PaginatedResultDto<CommentDto>>> GetComments(CommentQuery query) {
    IQueryable<Comment> commentsQuery = db.Comments
      .AsNoTracking();

    if (query.UserId != null) {
      commentsQuery = commentsQuery.Where(c => c.UserId == query.UserId);
    }

    if (query.ListingId != null) {
      commentsQuery = commentsQuery.Where(c => c.ListingId == query.ListingId);
    }

    int commentsCount = await commentsQuery.CountAsync();

    commentsQuery = commentsQuery
      .Take(query.Take)
      .Skip(query.Page * query.Take)
      .OrderByDescending(c => c.CreatedAt);

    List<Comment> comments = await commentsQuery.ToListAsync();

    return new PaginatedResultDto<CommentDto> {
      Items = comments.Select(c => mapper.Map<CommentDto>(c)).ToList(),
      TotalItems = commentsCount,
    };
  }

  public ValueTask<Comment?> FindComment(Guid commentId) {
    return db.Comments.FindAsync(commentId);
  }

  public async Task DeleteComment(Comment comment) {
    db.Remove(comment);
    await db.SaveChangesAsync();
  }
}
