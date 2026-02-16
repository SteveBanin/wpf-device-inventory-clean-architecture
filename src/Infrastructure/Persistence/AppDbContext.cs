using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;


/// <summary>
/// EF Core database context. This is the "session" to the database.
/// DbSet<Device> represents the Devices table.
/// </summary>
public class AppDbContext : DbContext
{
    public DbSet<Device> Devices => Set<Device>();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Device>(b =>
        {
            b.HasKey(x => x.Id);

            b.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(200);

            b.Property(x => x.SerialNumber)
                .IsRequired()
                .HasMaxLength(200);

            b.Property(x => x.Location)
                .IsRequired()
                .HasMaxLength(200);

            b.Property(x => x.LastServiceDate);
        });
    }
}
