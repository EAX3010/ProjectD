using Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            // Primary key
            builder.HasKey(p => p.Id);

            // Indexes with explicit names
            builder.HasIndex(p => p.Name)
                .HasDatabaseName("IX_Product_Name");

            builder.HasIndex(p => p.CategoryId)
                .HasDatabaseName("IX_Product_CategoryId");

            builder.HasIndex(p => p.Featured)
                .HasDatabaseName("IX_Product_Featured");

            // Relationships
            builder.HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            // Property configurations
            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(p => p.Price)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(p => p.Description)
                .HasMaxLength(500);

            builder.Property(p => p.Image)
                .HasMaxLength(250);

            builder.Property(p => p.StockQuantity)
                .IsRequired();

            builder.Property(p => p.Featured)
                .IsRequired();

            builder.Property(p => p.CreatedDate)
                .HasDefaultValueSql("GETUTCDATE()")
                .ValueGeneratedOnAdd();

            builder.Property(p => p.UpdatedDate)
                .HasDefaultValueSql("GETUTCDATE()")
                .ValueGeneratedOnAddOrUpdate();

            // Configure RowVersion as a concurrency token
            builder.Property(p => p.RowVersion)
                .IsRowVersion();
        }
    }
}
