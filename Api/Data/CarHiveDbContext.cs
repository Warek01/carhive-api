using Api.Enums;
using Api.Helpers;
using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Data;

public class CarHiveDbContext(DbContextOptions<CarHiveDbContext> options, IWebHostEnvironment env)
  : DbContext(options) {
  public const string TimestampNoTimezoneSql = "TIMESTAMP(1) WITHOUT TIME ZONE";
  private const string CurrentTimestampSql = "CURRENT_TIMESTAMP";
  private const string UuidGenSql = "UUID_GENERATE_V4()";

  public virtual DbSet<Listing> Listings { get; set; }
  public virtual DbSet<User> Users { get; set; }
  public virtual DbSet<Brand> Brands { get; set; }
  public virtual DbSet<Country> Countries { get; set; }
  public virtual DbSet<Model> Models { get; set; }
  public virtual DbSet<ListingUserFavorite> ListingUserFavorites { get; set; }
  public virtual DbSet<ListingActivity> ListingActivities { get; set; }
  public virtual DbSet<City> Cities { get; set; }
  public virtual DbSet<Currency> Currencies { get; set; }
  public virtual DbSet<Report> Reports { get; set; }

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
    optionsBuilder.UseNpgsql();

    if (env.IsDevelopment()) {
      optionsBuilder.EnableSensitiveDataLogging();
    }
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder) {
    modelBuilder.HasPostgresExtension("uuid-ossp");
    modelBuilder.HasPostgresExtension("pg_trgm");

    modelBuilder.HasPostgresEnum<CarBodyStyle>();
    modelBuilder.HasPostgresEnum<CarFuelType>();
    modelBuilder.HasPostgresEnum<CarColor>();
    modelBuilder.HasPostgresEnum<UserRole>();
    modelBuilder.HasPostgresEnum<CarStatus>();
    modelBuilder.HasPostgresEnum<CarDrivetrain>();
    modelBuilder.HasPostgresEnum<CarTransmission>();
    modelBuilder.HasPostgresEnum<ListingStatus>();
    modelBuilder.HasPostgresEnum<ListingAction>();
    modelBuilder.HasPostgresEnum<ReportType>();

    modelBuilder.Entity<Report>()
      .Property(e => e.Id)
      .HasDefaultValueSql(UuidGenSql);
    
    modelBuilder.Entity<Report>()
      .Property(e => e.CreatedAt)
      .HasDefaultValueSql(CurrentTimestampSql);

    modelBuilder.Entity<ListingActivity>()
      .Property(e => e.Id)
      .HasDefaultValueSql(UuidGenSql);
    
    modelBuilder.Entity<ListingActivity>()
      .Property(e => e.Timestamp)
      .HasDefaultValueSql(CurrentTimestampSql);
    
    modelBuilder.Entity<Listing>()
      .Property(e => e.Id)
      .HasDefaultValueSql(UuidGenSql);

    modelBuilder.Entity<Listing>()
      .Property(e => e.CreatedAt)
      .HasDefaultValueSql(CurrentTimestampSql);

    modelBuilder.Entity<Listing>()
      .Property(e => e.UpdatedAt)
      .HasDefaultValueSql(CurrentTimestampSql);

    modelBuilder.Entity<Listing>()
      .HasIndex(e => e.CreatedAt);

    modelBuilder.Entity<Listing>()
      .HasIndex(e => e.ProductionYear);

    modelBuilder.Entity<Listing>()
      .HasIndex(e => e.Price);

    modelBuilder.Entity<User>()
      .Property(e => e.Id)
      .HasDefaultValueSql(UuidGenSql);

    modelBuilder.Entity<User>()
      .HasMany(u => u.Listings)
      .WithOne(l => l.Publisher)
      .OnDelete(DeleteBehavior.Cascade);

    modelBuilder.Entity<User>()
      .Property(e => e.CreatedAt)
      .HasDefaultValueSql(CurrentTimestampSql);

    modelBuilder.Entity<User>()
      .Property(e => e.UpdatedAt)
      .HasDefaultValueSql(CurrentTimestampSql);

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
