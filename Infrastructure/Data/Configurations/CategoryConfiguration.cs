using Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            // Primary key configuration
            builder.HasKey(c => c.Id);

            // Index on Name with explicit name
            builder.HasIndex(c => c.Name)
                .HasDatabaseName("IX_Category_Name");

            // Property configurations
            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);

            // Relationships configuration
            builder.HasMany(c => c.Products)
                .WithOne(p => p.Category)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure RowVersion as a concurrency token
            builder.Property(c => c.RowVersion)
                .IsRowVersion(); // Simplified configuration
        }
    }
}
