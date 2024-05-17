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

  public IQueryable<User> GetUsers()
  {
    return _dbContext.Users.AsNoTracking();
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

  public async Task<User> RegisterUser(RegisterDto registerDto)
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
       UserRole.ListingCreator
    };

    await _dbContext.SaveChangesAsync();
    return user;
  }

  // To be called by admin when creating users in dashboard
  public async Task CreateUser(CreateUserDto c)
  {
    var user = new User
    {
      Username = c.Username,
      Password = c.Password,
      Email = c.Email,
      Roles = c.Roles
    };

    await _dbContext.AddAsync(user);
    await _dbContext.SaveChangesAsync();
  }

  public async Task<User?> FindUser(Guid userId)
  {
    return await _dbContext.Users.FindAsync(userId);
  }

  public async Task UpdateUser(User user, UpdateUserDto updateDto)
  {
    user.Username = updateDto.Username;
    user.Email = updateDto.Email;
    user.Roles = updateDto.Roles;
    user.UpdatedAt = DateTime.Now;
    await _dbContext.SaveChangesAsync();
  }
}
