using AutoMapper;
using FafCarsApi.Models;
using FafCarsApi.Models.Dto;
using FafCarsApi.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace FafCarsApi.Services;

public class UserService
{
  private readonly FafCarsDbContext _dbContext;
  private readonly ILogger<UserService> _logger;
  private readonly IConfiguration _config;

  public UserService(
    FafCarsDbContext dbContext,
    ILogger<UserService> logger,
    IConfiguration config
  )
  {
    _dbContext = dbContext;
    _logger = logger;
    _config = config;
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

  public async Task<User?> FindUserByUsername(string username)
  {
    return await _dbContext.Users
      .Where(u => u.Username == username)
      .FirstOrDefaultAsync();
  }

  public async Task<User> CreateUser(RegisterDto registerDto)
  {
    var user = new User();
    await _dbContext.Users.AddAsync(user);

    var config = new MapperConfiguration(cfg => cfg.CreateMap<RegisterDto, User>());
    IMapper mapper = config.CreateMapper();
    mapper.Map(registerDto, user);

    user.Password = BCrypt.Net.BCrypt.EnhancedHashPassword(
      user.Password,
      int.Parse(_config["BCrypt:HashRounds"]!)
    );
    user.Roles = new List<UserRole>
    {
      UserRole.User, UserRole.CreateListing, UserRole.RemoveListing
    };

    await _dbContext.SaveChangesAsync();
    return user;
  }

  public async Task<User?> FindUser(Guid userId)
  {
    return await _dbContext.Users.FindAsync(userId);
  }
}
