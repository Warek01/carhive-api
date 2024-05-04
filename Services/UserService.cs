using FafCarsApi.Models;
using FafCarsApi.Models.Dto;
using FafCarsApi.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace FafCarsApi.Services;

public class UserService
{
  private readonly FafCarsDbContext _dbContext;

  public UserService(FafCarsDbContext dbContext)
  {
    _dbContext = dbContext;
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
        Success = true,
        Error = "user not found"
      };

    _dbContext.Remove(user);
    await _dbContext.SaveChangesAsync();

    return new OperationResultDto
    {
      Success = true
    };
  }
}
