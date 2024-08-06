using Api.Enums;

namespace Api.Helpers;

public static class AuthRoles {
  public const string UserOnly = nameof(UserRole.User);
  public const string AdminOnly = nameof(UserRole.Admin);
  public const string SuperAdminOnly = nameof(UserRole.SuperAdmin);
  public const string User = UserOnly + ", " + AdminOnly + ", " + SuperAdminOnly;
  public const string Admin = AdminOnly + ", " + SuperAdminOnly;
  public const string SuperAdmin = SuperAdminOnly;
}
