using System.Linq.Expressions;
using Api.Data;
using Api.Dtos.Request;
using Api.Enums;
using Api.Models;
using AutoMapper;
using Google.Apis.Auth;
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
    User? user = await dbContext.Users.FindAsync(userId);

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
    var user = mapper.Map<User>(registerDto);

    user.Password = BCrypt.Net.BCrypt.EnhancedHashPassword(
      user.Password,
      int.Parse(config["BCrypt:HashRounds"]!)
    );

    user.Roles = [UserRole.User,];

    await dbContext.Users.AddAsync(user);
    await dbContext.SaveChangesAsync();
    return user;
  }

  public async Task<User> RegisterUser(GoogleJsonWebSignature.Payload payload, string username) {
    var user = new User {
      Provider = OauthIdentityProvider.Google,
      Email = payload.Email,
      Username = username,
      FirstName = payload.GivenName,
      LastName = payload.FamilyName,
      Picture = new Uri(payload.Picture),
      Roles = [UserRole.User,],
    };

    await dbContext.Users.AddAsync(user);
    await dbContext.SaveChangesAsync();
    return user;
  }

  // To be called by admin when creating users in dashboard
  public async Task CreateUser(CreateUserDto createDto) {
    var user = mapper.Map<User>(createDto);

    await dbContext.AddAsync(user);
    await dbContext.SaveChangesAsync();
  }

  public async Task<ActionResult> UpdateUserAsUser(User user, UpdateUserDto dto) {
    return await UpdateUser(user, dto);
  }

  public async Task<ActionResult> UpdateUserAsSuperAdmin(User user, UpdateUserDto dto) {
    if (dto.UpdateRoles) {
      if (dto.Roles == null) {
        return new BadRequestObjectResult("roles should not be null");
      }

      user.Roles = dto.Roles;
    }

    return await UpdateUser(user, dto);
  }

  public async Task<ActionResult> UpdateUserAsAdmin(User user, UpdateUserDto dto) {
    if (dto.UpdateStatus) {
      if (dto.Status == null) {
        return new BadRequestObjectResult("status should not be null");
      }

      user.Status = dto.Status.Value;
    }

    return await UpdateUser(user, dto);
  }

  public async Task<ActionResult> ClearFavorites(User user) {
    user.Favorites.Clear();
    await dbContext.SaveChangesAsync();
    return new OkResult();
  }

  private async Task<ActionResult> UpdateUser(User user, UpdateUserDto dto) {
    if (dto.UpdateUsername) {
      if (dto.Username == null) {
        return new BadRequestObjectResult("username should not be null");
      }

      user.Username = dto.Username;
    }

    if (dto.UpdateEmail) {
      if (dto.Email == null) {
        return new BadRequestObjectResult("email should not be null");
      }

      user.Email = dto.Email;
    }

    if (dto.UpdateFirstName) {
      user.FirstName = dto.FirstName;
    }

    if (dto.UpdateLastName) {
      user.LastName = dto.LastName;
    }

    if (dto.UpdatePicture) {
      user.Picture = dto.Picture;
    }

    user.UpdatedAt = DateTime.Now;
    await dbContext.SaveChangesAsync();

    return new NoContentResult();
  }
}
