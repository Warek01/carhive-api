using FafCarsApi.Models;
using Microsoft.EntityFrameworkCore;

namespace FafCarsApi.Data;

public class FafCarsDbContext : DbContext {
  public FafCarsDbContext() { }

  public FafCarsDbContext(DbContextOptions<FafCarsDbContext> options)
    : base(options) { }

  public virtual DbSet<Listing> Listings { get; set; }
  public virtual DbSet<User> Users { get; set; }

  protected override void OnModelCreating(ModelBuilder modelBuilder) {
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
      .HasData(User.InitialData);
  }
}
