using FafCarsApi.Models;
using FafCarsApi.Models.Dto;
using FafCarsApi.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace FafCarsApi.Services;

public class UserService
{
  private readonly FafCarsDbContext _dbContext;
  private readonly ILogger<UserService> _logger;

  public UserService(FafCarsDbContext dbContext, ILogger<UserService> logger)
  {
    _dbContext = dbContext;
    _logger = logger;
  }

  public async Task<ICollection<UserDto>> GetUsers(PaginationQuery pagination)
  {
    return await _dbContext.Users
      .Take(pagination.Take)
      .Skip(pagination.Take * pagination.Page)
      .Select(u => new UserDto
        {
          Id = u.Id,
          Username = u.Username
        }
      )
      .ToListAsync();
  }

  public async Task<OperationResultDto> DeleteUser(Guid userId)
  {
    User? user = await _dbContext.Users.FindAsync(userId);

    if (user == null)
      return new OperationResultDto
      {
        Success = false,
        Error = "user not found"
      };

    _dbContext.Remove(user);
    await _dbContext.SaveChangesAsync();
    
    _logger.LogInformation($"Deleted user {user.Username} ({user.Id})");

    return new OperationResultDto
    {
      Success = true
    };
  }
}
