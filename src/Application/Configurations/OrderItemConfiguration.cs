using Application.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Application.Configurations;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.Property(x => x.OrderId)
            .IsRequired();
        
        builder.Property(x => x.ProductId)
            .IsRequired();
        
        builder.Property(x => x.Title)
            .IsRequired();
        
        builder.Property(x => x.Price)
            .IsRequired();
        
        builder.Property(x => x.Quantity)
            .IsRequired();
        
        builder.Property(x => x.Total)
            .IsRequired();
        
        builder.HasOne(x => x.Order)
            .WithMany(x => x.OrderItems)
            .HasForeignKey(x => x.OrderId)
            .IsRequired();
        
        builder.HasOne(x => x.Product)
            .WithMany()
            .HasForeignKey(x => x.ProductId)
            .IsRequired();
    }
}
