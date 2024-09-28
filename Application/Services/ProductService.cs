using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.Models;
using Infrastructure.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Shared.DTOs;
using Shared.Response;
using static Shared.Response.Enums;

namespace Application.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ProductService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // Get all products
        public async Task<IEnumerable<ProductDto>> GetProductsAsync()
        {
            return await _context.Products
                .ProjectTo<ProductDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        // Get product by ID
        public async Task<ProductDto?> GetProductByIdAsync(int id)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == id);

            return _mapper.Map<ProductDto>(product);
        }

        // Add a new product
        public async Task<ServicesResponse<ProductDto>> AddProductAsync(ProductDto productDto)
        {
            // 1. Null Check
            if (productDto == null)
            {
                return new ServicesResponse<ProductDto>(ResponseType.Error, "Product is null", null);
            }

            // 2. Mapping DTO to Entity
            var product = _mapper.Map<Product>(productDto);

            try
            {
                // Add the new product
                await _context.Products.AddAsync(product);
                await _context.SaveChangesAsync();

                // Load the related Category
                await _context.Entry(product).Reference(p => p.Category).LoadAsync();

                // Map Entity back to DTO
                var productDtoResult = _mapper.Map<ProductDto>(product);

                return new ServicesResponse<ProductDto>(ResponseType.Success, "Product added successfully.", productDtoResult);
            }
            catch (DbUpdateException dbEx) when (IsUniqueConstraintViolation(dbEx))
            {
                // Handle unique constraint violation
                return new ServicesResponse<ProductDto>(ResponseType.Error, "A product with the same name already exists.", null);
            }
            catch (Exception ex)
            {
                // Log the exception
                //_logger.LogError(ex, "Error adding product.");

                return new ServicesResponse<ProductDto>(ResponseType.Error, "An error occurred while adding the product.", null);
            }
        }

        /// <summary>
        /// Determines if the exception is due to a unique constraint violation.
        /// This method needs to be implemented based on your database provider.
        /// </summary>
        private bool IsUniqueConstraintViolation(DbUpdateException exception)
        {
            // Example for SQL Server:
            if (exception.InnerException is SqlException sqlEx)
            {
                // Error number for unique constraint violation in SQL Server is 2627 or 2601
                return sqlEx.Number == 2627 || sqlEx.Number == 2601;
            }

            return false;
        }


        // Update an existing product
        public async Task<bool> UpdateProductAsync(ProductDto productDto)
        {
            var product = await _context.Products.FindAsync(productDto.Id);
            if (product == null)
                return false;

            // Copy the original RowVersion
            var originalRowVersion = product.RowVersion;

            _mapper.Map(productDto, product);

            // Set the original RowVersion
            _context.Entry(product).OriginalValues["RowVersion"] = originalRowVersion;

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                // Handle concurrency conflict
                return false;
            }
        }

        // Delete a product
        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return false;

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
