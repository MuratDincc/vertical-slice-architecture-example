using Application.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application;

public class ApplicationDbContext : DbContext
{
    // dotnet ef migrations add v1.0.0 -c Application.ApplicationDbContext -p ../Application
    // dotnet ef migrations script 0 v1.0.0 -c Application.TenantDbContext -p ../Application

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }

    public DbSet<Entities.Customer> Customer { get; set; }
    public DbSet<Entities.Product> Product { get; set; }
    public DbSet<Entities.Order> Order { get; set; }
    public DbSet<Entities.OrderItem> OrderItem { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("public");
        
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        modelBuilder.Entity<Entities.Customer>()
            .HasData(
                new Entities.Customer
                {
                    Id = 1,
                    Name = "Murat",
                    Surname = "Dinc",
                    Email = "info@muratdinc.dev"
                }
            );
        
        modelBuilder.Entity<Entities.Product>()
            .HasData(
                new Entities.Product
                {
                    Id = 1,
                    Title = "iPhone 8",
                    Price = 1000
                },
                new Entities.Product
                {
                    Id = 2,
                    Title = "iPhone X",
                    Price = 1300
                },
                new Entities.Product
                {
                    Id = 3,
                    Title = "iPhone 11",
                    Price = 1500
                }
            );
        
        modelBuilder.Entity<Entities.Order>()
            .HasData(
                new Entities.Order
                {
                    Id = 1,
                    CustomerId = 1,
                    Total = 6100
                }
            );
        
        modelBuilder.Entity<Entities.OrderItem>()
            .HasData(
                new OrderItem
                {
                    Id = 1,
                    OrderId = 1,
                    ProductId = 1,
                    Title = "iPhone 8",
                    Quantity = 2,
                    Price = 1000,
                    Total = 2000
                },
                new OrderItem
                {
                    Id = 2,
                    OrderId = 1,
                    ProductId = 2,
                    Title = "iPhone X",
                    Quantity = 2,
                    Price = 1300,
                    Total = 2600
                },
                new OrderItem
                {
                    Id = 3,
                    OrderId = 1,
                    ProductId = 3,
                    Title = "iPhone 11",
                    Quantity = 1,
                    Price = 1500,
                    Total = 1500
                }
            );
        
        base.OnModelCreating(modelBuilder);
    }
    
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        return await base.SaveChangesAsync(cancellationToken);
    }
}