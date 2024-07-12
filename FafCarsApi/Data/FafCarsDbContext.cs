using FafCarsApi.Enums;
using FafCarsApi.Helpers;
using FafCarsApi.Models;
using Microsoft.EntityFrameworkCore;

namespace FafCarsApi.Data;

public class FafCarsDbContext(DbContextOptions<FafCarsDbContext> options, IWebHostEnvironment env)
  : DbContext(options) {
  public const string TIMESTAMP_NO_TIMEZONE_SQL = "TIMESTAMP(1) WITHOUT TIME ZONE";

  private const string CURRENT_TIMESTAMP_SQL = "CURRENT_TIMESTAMP";
  private const string UUID_GEN_SQL = "UUID_GENERATE_V4()";

  public virtual DbSet<Listing> Listings { get; set; }
  public virtual DbSet<User> Users { get; set; }
  public virtual DbSet<Brand> Brands { get; set; }
  public virtual DbSet<Country> Countries { get; set; }
  public virtual DbSet<Model> Models { get; set; }
  public virtual DbSet<ListingUserFavorite> ListingUserFavorites { get; set; }

  protected override void OnModelCreating(ModelBuilder modelBuilder) {
    modelBuilder.HasPostgresExtension("uuid-ossp");

    modelBuilder.HasPostgresEnum<CarBodyStyle>();
    modelBuilder.HasPostgresEnum<CarFuelType>();
    modelBuilder.HasPostgresEnum<CarColor>();
    modelBuilder.HasPostgresEnum<UserRole>();
    modelBuilder.HasPostgresEnum<CarStatus>();
    modelBuilder.HasPostgresEnum<ListingStatus>();

    modelBuilder.Entity<Listing>()
      .Property(e => e.Id)  
      .HasDefaultValueSql(UUID_GEN_SQL);

    modelBuilder.Entity<Listing>()
      .Property(e => e.CreatedAt)
      .HasDefaultValueSql(CURRENT_TIMESTAMP_SQL);

    modelBuilder.Entity<Listing>()
      .Property(e => e.UpdatedAt)
      .HasDefaultValueSql(CURRENT_TIMESTAMP_SQL);

    modelBuilder.Entity<Listing>()
      .HasIndex(e => e.CreatedAt);

    modelBuilder.Entity<Listing>()
      .HasIndex(e => e.ProductionYear);

    modelBuilder.Entity<Listing>()
      .HasIndex(e => e.Price);

    modelBuilder.Entity<User>()
      .Property(e => e.Id)
      .HasDefaultValueSql(UUID_GEN_SQL);

    modelBuilder.Entity<User>()
      .HasMany(u => u.Listings)
      .WithOne(l => l.Publisher)
      .OnDelete(DeleteBehavior.Cascade);

    modelBuilder.Entity<User>()
      .Property(e => e.CreatedAt)
      .HasDefaultValueSql(CURRENT_TIMESTAMP_SQL);

    modelBuilder.Entity<User>()
      .Property(e => e.UpdatedAt)
      .HasDefaultValueSql(CURRENT_TIMESTAMP_SQL);

    modelBuilder.Entity<User>()
      .HasIndex(e => e.Username)
      .IsUnique();

    modelBuilder.Entity<User>()
      .HasIndex(e => e.CreatedAt);

    modelBuilder.Entity<User>()
      .HasMany(u => u.Favorites)
      .WithMany(l => l.UsersFavorites)
      .UsingEntity<ListingUserFavorite>();

    var initializer = new DbInitializer(modelBuilder, env);
    initializer.Initialize();
  }
}
