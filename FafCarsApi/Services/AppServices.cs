namespace FafCarsApi.Services;

public static class AppServices {
  private static readonly List<Type> ScopedServices = [
    typeof(ListingService),
    typeof(UserService),
    typeof(AuthService),
    typeof(BrandService),
    typeof(CountryService),
    typeof(ModelService),
    typeof(StatisticsService),
    typeof(CityService),
    typeof(CurrencyService),
    typeof(CacheService),
  ];

  private static readonly List<Type> SingletonServices = [
    typeof(StaticFileService),
  ];

  public static void Register(WebApplicationBuilder builder) {
    foreach (var serviceType in ScopedServices) {
      builder.Services.AddScoped(serviceType);
    }

    foreach (var serviceType in SingletonServices) {
      builder.Services.AddSingleton(serviceType);
    }
  }
}
