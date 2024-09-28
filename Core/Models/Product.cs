﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Models
{
    public class Product
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
        public int StockQuantity { get; set; } = 0;

        [Required]
        public bool Featured { get; set; } = false;

        [Required(ErrorMessage = "Category ID is required.")]
        public int CategoryId { get; set; }

        // Navigation property
        public Category? Category { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }

        public byte[]? RowVersion { get; set; }
    }
}
