using System.Linq.Expressions;
using Api.Data;
using Api.Dtos.Request;
using Api.Enums;
using Api.Models;
using AutoMapper;
using Api.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Services;

public class UserService(
  CarHiveDbContext dbContext,
  IConfiguration config,
  IMapper mapper
) {
  public IQueryable<User> GetUsers() {
    return dbContext.Users.AsNoTracking();
  }

  public async Task<User?> DeleteUser(Guid userId) {
    var user = await dbContext.Users.FindAsync(userId);

    if (user == null)
      return null;

    dbContext.Remove(user);
    await dbContext.SaveChangesAsync();

    return user;
  }

  public Task<User?> FindUser(
    Guid userId,
    bool includeFavorites = false,
    bool includeListings = false
  ) {
    return FindUser(u => u.Id == userId, includeFavorites, includeListings);
  }

  public Task<User?> FindUserByUsername(
    string username,
    bool includeFavorites = false,
    bool includeListings = false
  ) {
    return FindUser(u => u.Username == username, includeFavorites, includeListings);
  }

  public Task<User?> FindUser(
    Expression<Func<User, bool>> conditionFn,
    bool includeFavorites = false,
    bool includeListings = false
  ) {
    IQueryable<User> query = dbContext.Users.AsQueryable();

    if (includeFavorites)
      query = query.Include(u => u.Favorites);

    if (includeListings)
      query = query.Include(u => u.Listings);

    return query.FirstOrDefaultAsync(conditionFn);
  }

  public async Task<User> RegisterUser(RegisterDto registerDto) {
    User user = mapper.Map<User>(registerDto);

    user.Password = BCrypt.Net.BCrypt.EnhancedHashPassword(
      user.Password,
      int.Parse(config["BCrypt:HashRounds"]!)
    );

    user.Roles = [UserRole.User];

    await dbContext.Users.AddAsync(user);
    await dbContext.SaveChangesAsync();
    return user;
  }

  // To be called by admin when creating users in dashboard
  public async Task CreateUser(CreateUserDto createDto) {
    User user = mapper.Map<User>(createDto);

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

  public async Task<ActionResult> ClearFavorites(User user) {
    user.Favorites.Clear();
    await dbContext.SaveChangesAsync();
    return new OkResult();
  }
}
