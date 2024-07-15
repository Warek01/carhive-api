using System.Text.Json;
using System.Text.Json.Serialization;
using FafCarsApi.Enums;
using FafCarsApi.Models;
using Microsoft.EntityFrameworkCore;
using static BCrypt.Net.BCrypt;

namespace FafCarsApi.Helpers;

public class DbInitializer(ModelBuilder modelBuilder, IWebHostEnvironment env) {
  private abstract class JsonResource;

  private class JsonResourceUser : JsonResource {
    public string Id { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Email { get; set; } = null!;
    public List<string> Roles { get; set; } = null!;
    public string CreatedAt { get; set; } = null!;
    public string? DeletedAt { get; set; }
    public string UpdatedAt { get; set; } = null!;
    public string? PhoneNumber { get; set; }
  }

  private class JsonResourceCountry : JsonResource {
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
  }

  private class JsonResourceBrand : JsonResource {
    public string CountryCode { get; set; } = null!;
    public string Name { get; set; } = null!;
  }

  private class JsonResourceModel : JsonResource {
    public string BrandName { get; set; } = null!;
    public string Name { get; set; } = null!;
  }

  private class JsonResourceListing : JsonResource {
    public string Id { get; set; } = null!;
    public string BrandName { get; set; } = null!;
    public string ModelName { get; set; } = null!;
    public double Price { get; set; }
    public string BodyStyle { get; set; } = null!;
    public int? Horsepower { get; set; }
    public double? EngineVolume { get; set; }
    public string EngineType { get; set; } = null!;
    public string Color { get; set; } = null!;
    public int Clearance { get; set; }
    public int WheelSize { get; set; }
    public int Mileage { get; set; }
    public int ProductionYear { get; set; }
    public List<string> Images { get; set; } = [];
    public string PublisherId { get; set; } = null!;
    public string? DeletedAt { get; set; } = null!;
    public string CreatedAt { get; set; } = null!;
    public string UpdatedAt { get; set; } = null!;
    public string CountryCode { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string City { get; set; } = null!;
  }

  private class JsonResourceListingUserFavorite : JsonResource {
    public string UserId { get; set; } = null!;
    public string ListingId { get; set; } = null!;
  }

  private static readonly JsonSerializerOptions _serializerOptions = new() {
    WriteIndented = false,
    PropertyNameCaseInsensitive = false,
    AllowTrailingCommas = true,
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
    UnmappedMemberHandling = JsonUnmappedMemberHandling.Disallow,
  };

  public void Initialize() {
    List<Action<ModelBuilder>> populateFunctions = [PopulateCountries, PopulateBrands, PopulateModels];
    List<Action<ModelBuilder>> devPopulateFunctions = [PopulateUsers, PopulateListings, PopulateListingUserFavorites];
    List<Action<ModelBuilder>> all = env.IsDevelopment()
      ? populateFunctions.Concat(devPopulateFunctions).ToList()
      : populateFunctions;

    foreach (var fn in all)
      fn(modelBuilder);
  }

  private static List<T> ReadArrayResourceFile<T>(string fileName) where T : JsonResource {
    string path = Path.Combine("Resources", fileName + ".json");
    Console.WriteLine($"Reading array resource file \"{path}\"");

    if (!File.Exists(path))
      throw new Exception($"resource file does not exist \"{path}\"");

    FileStream stream = File.OpenRead(path);
    stream.Seek(0, 0);

    List<T>? list = JsonSerializer.Deserialize<List<T>>(stream, _serializerOptions);

    if (list == null)
      throw new Exception($"invalid array resource file \"{path}\"");

    return list;
  }

  private static void PopulateUsers(ModelBuilder modelBuilder) {
    List<JsonResourceUser> resourceUsers = ReadArrayResourceFile<JsonResourceUser>("users.development");
    var users = resourceUsers.Select(
      u => new User {
        Id = new Guid(u.Id),
        CreatedAt = DateTime.Parse(u.CreatedAt),
        UpdatedAt = DateTime.Parse(u.CreatedAt),
        DeletedAt = u.DeletedAt == null ? null : DateTime.Parse(u.DeletedAt),
        PhoneNumber = u.PhoneNumber,
        Username = u.Username,
        Email = u.Email,
        Password = EnhancedHashPassword(u.Password, 13),
        Roles = u.Roles.Select(role => role switch {
            "Admin" => UserRole.Admin,
            "ListingCreator" => UserRole.ListingCreator,
            _ => throw new ArgumentOutOfRangeException(nameof(role), role, null)
          })
          .ToList(),
      }
    );

    modelBuilder.Entity<User>()
      .HasData(users);
  }

  private static void PopulateCountries(ModelBuilder modelBuilder) {
    List<JsonResourceCountry> resourceCountries = ReadArrayResourceFile<JsonResourceCountry>("countries");
    var countries = resourceCountries.Select(
      c => new Country {
        Code = c.Code,
        Name = c.Name,
      }
    );

    modelBuilder.Entity<Country>()
      .HasData(countries);
  }

  private static void PopulateBrands(ModelBuilder modelBuilder) {
    List<JsonResourceBrand> resourceCountries = ReadArrayResourceFile<JsonResourceBrand>("brands");
    var brands = resourceCountries.Select(
      c => new Brand {
        CountryCode = c.CountryCode,
        Name = c.Name,
      }
    );

    modelBuilder.Entity<Brand>()
      .HasData(brands);
  }

  private static void PopulateModels(ModelBuilder modelBuilder) {
    List<JsonResourceModel> resourceModels = ReadArrayResourceFile<JsonResourceModel>("models");
    var models = resourceModels.Select(
      m => new Model {
        Name = m.Name,
        BrandName = m.BrandName,
      }
    );

    modelBuilder.Entity<Model>()
      .HasData(models);
  }

  private static void PopulateListings(ModelBuilder modelBuilder) {
    List<JsonResourceListing> resourceListings = ReadArrayResourceFile<JsonResourceListing>("listings.development");
    var listings = resourceListings.Select(
      l => new Listing {
        Id = new Guid(l.Id),
        CreatedAt = DateTime.Parse(l.CreatedAt),
        UpdatedAt = DateTime.Parse(l.UpdatedAt),
        DeletedAt = l.DeletedAt == null ? null : DateTime.Parse(l.DeletedAt),
        BrandName = l.BrandName,
        Images = l.Images,
        Clearance = l.Clearance,
        WheelSize = l.WheelSize,
        Mileage = l.Mileage,
        Price = l.Price,
        Color = Enum.Parse<CarColor>(l.Color, ignoreCase: true),
        BodyStyle = Enum.Parse<CarBodyStyle>(l.BodyStyle, ignoreCase: true),
        CountryCode = l.CountryCode,
        FuelType = Enum.Parse<CarFuelType>(l.EngineType, ignoreCase: true),
        Horsepower = l.Horsepower,
        SellAddress = l.Address,
        PublisherId = new Guid(l.PublisherId),
        ProductionYear = l.ProductionYear,
        ModelName = l.ModelName,
        City = l.City,
        EngineVolume = l.EngineVolume,
      }
    );

    modelBuilder.Entity<Listing>()
      .HasData(listings);
  }

  private static void PopulateListingUserFavorites(ModelBuilder modelBuilder) {
    List<JsonResourceListingUserFavorite> resourceFavorites =
      ReadArrayResourceFile<JsonResourceListingUserFavorite>("listingUserFavorite.development");
    var favorites = resourceFavorites.Select(
      f => new ListingUserFavorite {
        UserId = new Guid(f.UserId),
        ListingId = new Guid(f.ListingId),
      }
    );

    modelBuilder.Entity<ListingUserFavorite>()
      .HasData(favorites);
  }
}
