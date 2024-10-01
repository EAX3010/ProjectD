using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Shared.DTOs
{
    public class ProductDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Product name is required.")]
        [StringLength(100, ErrorMessage = "Product name can't exceed 100 characters.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be positive.")]
        public decimal Price { get; set; }

        [StringLength(500, ErrorMessage = "Description can't exceed 500 characters.")]
        public string? Description { get; set; }

        [StringLength(250, ErrorMessage = "Image URL can't exceed 250 characters.")]
        public string? Image { get; set; }

        [Required(ErrorMessage = "Stock quantity is required.")]
        [Range(0, int.MaxValue, ErrorMessage = "Stock quantity cannot be negative.")]
        public int? StockQuantity { get; set; }

        [Required(ErrorMessage = "Featured is required")]
        public bool Featured { get; set; } = false;

        [Required(ErrorMessage = "CategoryId is required.")]
        public int? CategoryId { get; set; }

        // Navigation property`
        public string CategoryName { get; set; } = string.Empty;

        public byte[]? RowVersion { get; set; }
    }
}
