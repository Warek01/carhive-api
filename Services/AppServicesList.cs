namespace FafCarsApi.Services;

public static class AppServicesList {
  private static readonly List<Type> Services = [
    typeof(ListingService),
    typeof(UserService),
    typeof(AuthService),
    typeof(StaticFileService),
  ];


  public static void Register(WebApplicationBuilder builder) {
    foreach (var serviceType in Services)
      builder.Services.AddScoped(serviceType);
  }
}
