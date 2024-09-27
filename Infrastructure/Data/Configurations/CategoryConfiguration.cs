using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Core.Models;
namespace Infrastructure.Data.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            // Configure the Name property
            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);

            // Configure the relationship with Product
            builder.HasMany(c => c.Products)
                .WithOne(p => p.Category)
                .HasForeignKey(p => p.CategoryId);

            // Optionally, configure indexes
            builder.HasIndex(c => c.Name);



        }
    }
}
