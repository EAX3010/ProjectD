using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Core.Models
{
    public class Category
    {
        public required int? Id { get; set; }
        public required string? Name { get; set; } = string.Empty;

        //navigation
        public required ICollection<Product>? Products { get; set; }
        public required byte[]? RowVersion { get; set; }
    }
}
