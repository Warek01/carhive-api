using FafCarsApi.Enums;
using FafCarsApi.Models;
using Microsoft.EntityFrameworkCore;

namespace FafCarsApi.Helpers;

public static class DbInitializer {
  public static readonly IList<string> BrandNames = [
    "Acura",
    "Alfa Romeo",
    "Audi",
    "Bentley",
    "BMW",
    "Bugatti",
    "Buick",
    "BYD",
    "Cadillac",
    "Chery",
    "Chevrolet",
    "Chrysler",
    "Daihatsu",
    "Dodge",
    "Ferrari",
    "Fiat",
    "Ford",
    "Genesis",
    "Geely",
    "GMC",
    "Honda",
    "Hummer",
    "Hyundai",
    "Infiniti",
    "Jaguar",
    "Jeep",
    "Kia",
    "Koenigsegg",
    "Lada",
    "Lamborghini",
    "Land Rover",
    "Lexus",
    "Lincoln",
    "Lotus",
    "Maserati",
    "Maybach",
    "Mazda",
    "McLaren",
    "Mercedes-Benz",
    "Mini",
    "Mitsubishi",
    "Nissan",
    "Oldsmobile",
    "Pagani",
    "Pontiac",
    "Porsche",
    "Proton",
    "Ram",
    "Rolls-Royce",
    "Saab",
    "Saturn",
    "Smart",
    "Spyker",
    "Subaru",
    "Suzuki",
    "Tesla",
    "Toyota",
    "Volkswagen",
    "Volvo"
  ];

  public static readonly IList<User> Users = [
    new User {
      Username = "admin",
      Password = BCrypt.Net.BCrypt.EnhancedHashPassword(
        "admin",
        13
      ),
      Email = "admin@gmail.com",
      Roles = [
        UserRole.Admin,
        UserRole.ListingCreator,
      ],
      CreatedAt = DateTime.Now,
      UpdatedAt = DateTime.Now,
      DeletedAt = null,
      PhoneNumber = "+37378000111",
      Listings = []
    },
    new User {
      Username = "user",
      Password = BCrypt.Net.BCrypt.EnhancedHashPassword(
        "user",
        13
      ),
      Email = "user@gmail.com",
      Roles = [
        UserRole.Admin
      ],
      CreatedAt = DateTime.Now,
      UpdatedAt = DateTime.Now,
      DeletedAt = null,
      PhoneNumber = "+37378111222",
      Listings = []
    }
  ];

  public static readonly IList<Listing> Listings = [];

  public static void Initialize(ModelBuilder modelBuilder) {
    modelBuilder
      .Entity<Brand>()
      .HasData(
        BrandNames.Select<string, Brand>(name => new Brand {
            Name = name
          }
        )
      );

    modelBuilder
      .Entity<User>()
      .HasData(
        Users.Select<User, User>(u => {
          u.Id = Guid.NewGuid();
          return u;
        })
      );

    modelBuilder
      .Entity<Listing>()
      .HasData(
        Listings.Select<Listing, Listing>(l => {
          l.Id = Guid.NewGuid();
          return l;
        })
      );
  }
}
