namespace FafCarsApi.Services;

public static class Services
{
  private static readonly List<Type> _services = new()
  {
    typeof(ListingService),
    typeof(UserService)
  };


  public static void Register(WebApplicationBuilder builder)
  {
    foreach (var serviceType in _services)
      builder.Services.AddScoped(serviceType);
  }
}
