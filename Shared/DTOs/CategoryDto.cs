using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs
{
    public class CategoryDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Category name is required.")]
        [StringLength(100, ErrorMessage = "Category name can't exceed 100 characters.")]
        public string Name { get; set; } = string.Empty;

        //navigation
        public required ICollection<ProductDto>? Products { get; set; }
        public byte[]? RowVersion { get; set; }
    }
}
