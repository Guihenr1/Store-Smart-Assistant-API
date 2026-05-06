using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StoreSmart.Domain.Entities;

namespace StoreSmart.Infrastructure.Persistence.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.SKU)
               .IsRequired()
               .HasMaxLength(50);

        builder.Property(p => p.Name)
               .IsRequired()
               .HasMaxLength(200);

        builder.Property(p => p.Description)
               .IsRequired()
               .HasMaxLength(2000);

        builder.Property(p => p.Brand)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(p => p.StockQuantity).IsRequired();
        builder.Property(p => p.IsActive).IsRequired();

        builder.Property(p => p.ImageUrl).HasMaxLength(500);
        builder.Property(p => p.Specifications).HasMaxLength(4000);
        builder.Property(p => p.Features).HasMaxLength(4000);
        builder.Property(p => p.TechnicalDetails).HasMaxLength(4000);

        builder.OwnsOne(p => p.Price, money =>
        {
            money.Property(m => m.Amount)
                 .HasColumnName("PriceAmount")
                 .HasPrecision(18, 2)
                 .IsRequired();

            money.Property(m => m.Currency)
                 .HasColumnName("Currency")
                 .HasMaxLength(3)
                 .IsRequired();
        });

        builder.OwnsOne(p => p.Category, category =>
        {
            category.Property(c => c.Name)
                    .HasColumnName("CategoryName")
                    .IsRequired()
                    .HasMaxLength(100);

            category.Property(c => c.Description)
                    .HasColumnName("CategoryDescription")
                    .HasMaxLength(500);
        });
        
        builder.Property(p => p.Tags)
               .HasConversion(
                   v => string.Join(',', v),
                   v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList())
               .HasMaxLength(1000);

        builder.Property(p => p.CreatedAt).IsRequired();
        builder.Property(p => p.UpdatedAt);

        builder.HasIndex(p => p.SKU).IsUnique();
        builder.HasIndex(p => p.Name);
        builder.HasIndex(p => p.Brand);
        builder.HasIndex(p => p.IsActive);

        builder.HasIndex(p => new { p.Brand, p.IsActive });
    }
}