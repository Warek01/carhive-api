using System.Linq.Expressions;
using AutoMapper;
using FafCarsApi.Dtos;
using FafCarsApi.Enums;
using FafCarsApi.Models;
using Microsoft.EntityFrameworkCore;

namespace FafCarsApi.Services;

public class UserService(
  FafCarsDbContext dbContext,
  ILogger<UserService> logger,
  IConfiguration config,
  ListingService listingService
) {
  public IQueryable<User> GetUsers() {
    return dbContext.Users.AsNoTracking();
  }

  public async Task<OperationResultDto> DeleteUser(Guid userId) {
    var user = await dbContext.Users.FindAsync(userId);

    if (user == null)
      return new OperationResultDto {
        Success = false,
        Error = "user not found"
      };

    dbContext.Remove(user);
    await dbContext.SaveChangesAsync();

    logger.LogInformation($"Deleted user {user.Username} ({user.Id})");

    return new OperationResultDto {
      Success = true
    };
  }

  public async Task<User?> FindUser(
    Guid userId,
    bool includeFavorites = false,
    bool includeListings = false
  ) {
    return await FindUser(u => u.Id == userId, includeFavorites, includeListings);
  }

  public async Task<User?> FindUserByUsername(
    string username,
    bool includeFavorites = false,
    bool includeListings = false
  ) {
    return await FindUser(u => u.Username == username, includeFavorites, includeListings);
  }

  public async Task<User?> FindUser(
    Expression<Func<User, bool>> conditionFn,
    bool includeFavorites = false,
    bool includeListings = false
  ) {
    IQueryable<User> query = dbContext.Users.AsQueryable();

    if (includeFavorites)
      query = query.Include(u => u.Favorites);

    if (includeListings)
      query = query.Include(u => u.Listings);

    return await query.FirstOrDefaultAsync(conditionFn);
  }

  public async Task<User> RegisterUser(RegisterDto registerDto) {
    var user = new User();
    await dbContext.Users.AddAsync(user);

    var config1 = new MapperConfiguration(cfg => cfg.CreateMap<RegisterDto, User>());
    var mapper = config1.CreateMapper();
    mapper.Map(registerDto, user);

    user.Password = BCrypt.Net.BCrypt.EnhancedHashPassword(
      user.Password,
      int.Parse(config["BCrypt:HashRounds"]!)
    );
    user.Roles = new List<UserRole> {
      UserRole.ListingCreator
    };

    await dbContext.SaveChangesAsync();
    return user;
  }

  // To be called by admin when creating users in dashboard
  public async Task CreateUser(CreateUserDto c) {
    var user = new User {
      Username = c.Username,
      Password = c.Password,
      Email = c.Email,
      Roles = c.Roles
    };

    await dbContext.AddAsync(user);
    await dbContext.SaveChangesAsync();
  }

  public async Task UpdateUser(User user, UpdateUserDto updateDto) {
    user.Username = updateDto.Username;
    user.Email = updateDto.Email;
    user.Roles = updateDto.Roles;
    user.UpdatedAt = DateTime.Now;
    await dbContext.SaveChangesAsync();
  }

  public async Task AddListingToFavorites(User user, Listing listing) {
    user.Favorites.Add(listing);
    await dbContext.SaveChangesAsync();
  }

  public async Task RemoveListingFromFavorites(User user, Listing listing) {
    user.Favorites.Remove(listing);
    await dbContext.SaveChangesAsync();
  }

  public async Task ClearFavorites(User user) {
    user.Favorites.Clear();
    await dbContext.SaveChangesAsync();
  }
}
