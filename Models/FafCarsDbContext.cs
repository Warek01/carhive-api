using FafCarsApi.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace FafCarsApi.Models;

public partial class FafCarsDbContext : DbContext
{
  public virtual DbSet<Listing> Listings { get; set; }
  public virtual DbSet<User> Users { get; set; }

  public FafCarsDbContext()
  {
  }

  public FafCarsDbContext(DbContextOptions<FafCarsDbContext> options)
    : base(options)
  {
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<Listing>()
      .Property(e => e.Id)
      .HasDefaultValueSql("uuid_generate_v4()");

    modelBuilder.Entity<Listing>()
      .Property(e => e.CreatedAt)
      .HasDefaultValueSql("CURRENT_TIMESTAMP");

    modelBuilder.Entity<Listing>()
      .Property(e => e.UpdatedAt)
      .HasDefaultValueSql("CURRENT_TIMESTAMP");

    modelBuilder.Entity<User>()
      .Property(e => e.Id)
      .HasDefaultValueSql("uuid_generate_v4()");

    modelBuilder.Entity<User>()
      .HasMany(u => u.Listings)
      .WithOne(l => l.Publisher)
      .OnDelete(DeleteBehavior.Cascade);

    modelBuilder.Entity<Listing>()
      .Property(e => e.CreatedAt)
      .HasDefaultValueSql("CURRENT_TIMESTAMP");

    modelBuilder.Entity<Listing>()
      .Property(e => e.UpdatedAt)
      .HasDefaultValueSql("CURRENT_TIMESTAMP");

    modelBuilder.Entity<User>()
      .HasIndex(e => e.Username)
      .IsUnique();

    modelBuilder.Entity<User>()
      .HasData(User.InitialData);
  }
}
