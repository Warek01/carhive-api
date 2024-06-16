﻿using FafCarsApi.Enums;
using Microsoft.EntityFrameworkCore;

namespace FafCarsApi.Models;

public class FafCarsDbContext : DbContext {
  public FafCarsDbContext() { }

  public FafCarsDbContext(DbContextOptions<FafCarsDbContext> options)
    : base(options) { }

  static FafCarsDbContext() { }

  public virtual DbSet<Listing> Listings { get; set; }
  public virtual DbSet<User> Users { get; set; }
  public virtual DbSet<Brand> Brands { get; set; }
  public virtual DbSet<Country> Countries { get; set; }

  public virtual DbSet<City> Cities { get; set; }
  public virtual DbSet<Model> Models { get; set; }

  protected override void OnModelCreating(ModelBuilder modelBuilder) {
    modelBuilder.HasPostgresEnum<BodyStyle>();
    modelBuilder.HasPostgresEnum<EngineType>();
    modelBuilder.HasPostgresEnum<CarColor>();
    modelBuilder.HasPostgresEnum<UserRole>();

    modelBuilder.Entity<Brand>()
      .HasMany(e => e.Models)
      .WithMany(e => e.Brands)
      .UsingEntity("ModelsBrands");

    modelBuilder.Entity<Listing>()
      .Property(e => e.Id)
      .HasDefaultValueSql("UUID_GENERATE_V4()");

    modelBuilder.Entity<Listing>()
      .Property(e => e.CreatedAt)
      .HasDefaultValueSql("CURRENT_TIMESTAMP");

    modelBuilder.Entity<Listing>()
      .Property(e => e.UpdatedAt)
      .HasDefaultValueSql("CURRENT_TIMESTAMP");

    modelBuilder.Entity<Listing>()
      .HasIndex(e => e.CreatedAt);

    modelBuilder.Entity<Listing>()
      .HasIndex(e => e.Year);

    modelBuilder.Entity<Listing>()
      .HasIndex(e => e.Price);

    modelBuilder.Entity<User>()
      .Property(e => e.Id)
      .HasDefaultValueSql("UUID_GENERATE_V4()");

    modelBuilder.Entity<User>()
      .HasMany(u => u.Listings)
      .WithOne(l => l.Publisher)
      .OnDelete(DeleteBehavior.Cascade);

    modelBuilder.Entity<User>()
      .Property(e => e.CreatedAt)
      .HasDefaultValueSql("CURRENT_TIMESTAMP");

    modelBuilder.Entity<User>()
      .Property(e => e.UpdatedAt)
      .HasDefaultValueSql("CURRENT_TIMESTAMP");

    modelBuilder.Entity<User>()
      .HasIndex(e => e.Username)
      .IsUnique();

    modelBuilder.Entity<User>()
      .HasIndex(e => e.CreatedAt);

    modelBuilder.Entity<User>()
      .HasMany(u => u.Favorites)
      .WithMany(l => l.UsersFavorites)
      .UsingEntity("UsersFavoriteListings");
  }
}
