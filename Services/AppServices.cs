namespace FafCarsApi.Services;

public static class AppServices
{
  private static readonly List<Type> _services = new()
  {
    typeof(ListingService),
    typeof(UserService),
    typeof(AuthService),
  };


  public static void Register(WebApplicationBuilder builder)
  {
    foreach (var serviceType in _services)
      builder.Services.AddScoped(serviceType);
  }
}
