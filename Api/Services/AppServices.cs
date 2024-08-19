namespace Api.Services;

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
    typeof(ReportService),
    typeof(CommentService),
  ];

  private static readonly List<Type> SingletonServices = [
    typeof(StaticFileService),
    typeof(ListingMappingService),
    typeof(ImageService),
  ];

  public static void Register(WebApplicationBuilder builder) {
    foreach (Type? serviceType in ScopedServices) {
      builder.Services.AddScoped(serviceType);
    }

    foreach (Type? serviceType in SingletonServices) {
      builder.Services.AddSingleton(serviceType);
    }
  }
}
