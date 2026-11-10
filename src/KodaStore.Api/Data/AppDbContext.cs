using KodaStore.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace KodaStore.Api.Data;

public sealed class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Products> Products => Set<Products>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Products>(entity =>
        {
            entity.ToTable("Products", "dbo", table =>
            {
                table.HasCheckConstraint("CK_Product_Price", "[Price] >= 0");
                table.HasCheckConstraint("CK_Product_Stock", "[Stock] >= 0");
            });

            entity.HasKey(p => p.ProductCode);

            entity.Property(p => p.ProductCode)
                .HasMaxLength(30)
                .IsRequired()
                .ValueGeneratedNever();
            
            entity.Property(p => p.Price)
                .IsRequired()
                .HasPrecision(18,2);

            entity.Property(p => p.Stock)
                .IsRequired();
            
            entity.Property(p => p.Category)
                .HasMaxLength(30)
                .IsRequired();
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.ToTable("Order", "dbo", table =>
            {
                table.HasCheckConstraint("CK_Orders_Status", "[Status] in ('Pending', 'Confirmed')");
            });

            entity.HasKey(p => p.Id);

            entity.Property(p => p.Id).ValueGeneratedOnAdd();

            entity.Property(p => p.OrderNumber).HasMaxLength(35).IsRequired();

            entity.Property(p => p.OrderDate).HasColumnType("datetimeoffset").IsRequired();

            entity.Property(p => p.Status).HasMaxLength(20).IsRequired();

            entity.HasIndex(p => p.OrderDate);
        });

        modelBuilder.Entity<OrderItem>(entity =>)
    }
}