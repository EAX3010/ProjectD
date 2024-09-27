using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Core;
using Core.Models;
namespace Infrastructure.Data.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            // Configure the Name property
            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(100);

            // Configure the Price property
            builder.Property(p => p.Price)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            // Configure the Description property
            builder.Property(p => p.Description)
                .HasMaxLength(500);

            // Configure the Image property
            builder.Property(p => p.Image)
                .HasMaxLength(250);

            // Configure the StockQuantity property with default value
            builder.Property(p => p.StockQuantity)
                .IsRequired()
                .HasDefaultValue(0);

            // Configure the CreatedDate property with default value
            builder.Property(p => p.CreatedDate)
                .HasDefaultValueSql("GETUTCDATE()")
                .ValueGeneratedOnAdd();

            // Configure the UpdatedDate property with default value
            builder.Property(p => p.UpdatedDate)
                .HasDefaultValueSql("GETUTCDATE()")
                .ValueGeneratedOnAddOrUpdate();

            // Configure the Featured property with default value
            builder.Property(p => p.Featured)
                .IsRequired()
                .HasDefaultValue(false);

            // Configure the relationship between Product and Category
            builder.HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .IsRequired();

            // Configure indexes for performance
            builder.HasIndex(p => p.Name);
            builder.HasIndex(p => p.CategoryId);
            builder.HasIndex(p => p.Featured);

            //configure a concurrency token
            builder.Property(p => p.RowVersion)
                    .IsRowVersion();

            builder.HasData(
    new Product
    {
        Id = 1,
        Name = "Sample Product",
        Price = 9.99m,
        StockQuantity = 100,
        CategoryId = 1,
        Featured = true
    }
);
        }
    }
}
