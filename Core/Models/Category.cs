using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Core.Models
{
    public class Category
    {
        // Primary Key
        public int Id { get; set; }

        // Category name with validation attributes
        [Required(ErrorMessage = "Category name is required.")]
        [StringLength(100, ErrorMessage = "Category name can't exceed 100 characters.")]
        public string Name { get; set; } = string.Empty;

        // Navigation property to Products
        public ICollection<Product>? Products { get; set; }

        // Concurrency token for optimistic concurrency control
        public byte[]? RowVersion { get; set; }
    }
}
