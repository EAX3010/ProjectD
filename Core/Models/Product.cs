using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Models
{
    public class Product
    {
        public required int? Id { get; set; }

        public required string? Name { get; set; }

        public required decimal? Price { get; set; }

        public required string? Description { get; set; }

        public required string? Image { get; set; }

        public required int? StockQuantity { get; set; }

        public required bool? Featured { get; set; } 

        public required int? CategoryId { get; set; }

        public required Category? Category { get; set; }

        public required DateTime? CreatedDate { get; set; }

        public required DateTime? UpdatedDate { get; set; }

        public required byte[]? RowVersion { get; set; }
    }
}
