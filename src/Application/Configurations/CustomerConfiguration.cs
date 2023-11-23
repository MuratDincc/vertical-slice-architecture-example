using Application.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Application.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.Property(x => x.Name)
            .HasMaxLength(200)
            .IsRequired();
        
        builder.Property(x => x.Surname)
            .HasMaxLength(200)
            .IsRequired();
        
        builder.Property(x => x.Email)
            .HasMaxLength(350)
            .IsRequired();
    }
}
