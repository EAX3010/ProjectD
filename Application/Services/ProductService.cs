using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.DTOs;
using Shared.Response;
using static Shared.Response.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductService> _logger;

        public ProductService(AppDbContext context, IMapper mapper, ILogger<ProductService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ServerResponse<IEnumerable<ProductDto>>> GetProductsAsync()
        {
            try
            {
                var products = await _context.Products
                    .AsNoTracking() // Improves performance for read-only operations
                    .ProjectTo<ProductDto>(_mapper.ConfigurationProvider)
                    .ToListAsync();

                _logger.LogTrace("Products retrieved.");
                return new ServerResponse<IEnumerable<ProductDto>>(ResponseType.Success, "Products retrieved.", products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving products.");
                return new ServerResponse<IEnumerable<ProductDto>>(ResponseType.Error, "An error occurred while retrieving products.", null);
            }
        }

        public async Task<ServerResponse<ProductDto>> GetProductByIdAsync(int id)
        {
            if (id <= 0)
            {
                return new ServerResponse<ProductDto>(ResponseType.Error, "Invalid product ID.", null);
            }

            try
            {
                var product = await _context.Products
                    .AsNoTracking()
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (product == null)
                    return new ServerResponse<ProductDto>(ResponseType.Error, "Product not found.", null);

                _logger.LogTrace("Product retrieved.");
                return new ServerResponse<ProductDto>(ResponseType.Success, "Product retrieved.", _mapper.Map<ProductDto>(product));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the product.");
                return new ServerResponse<ProductDto>(ResponseType.Error, "An error occurred while retrieving the product.", null);
            }
        }

        public async Task<ServerResponse<ProductDto>> AddProductAsync(ProductDto productDto)
        {
            if (productDto == null)
            {
                return new ServerResponse<ProductDto>(ResponseType.Error, "Product data is null.", null);
            }

            if (productDto.Id != 0)
            {
                return new ServerResponse<ProductDto>(ResponseType.Error, "Product can't be added with a pre-defined ID.", null);
            }

            if (string.IsNullOrWhiteSpace(productDto.Name))
            {
                return new ServerResponse<ProductDto>(ResponseType.Error, "Product name is required.", null);
            }

            if (productDto.Price < 0)
            {
                return new ServerResponse<ProductDto>(ResponseType.Error, "Product price cannot be negative.", null);
            }

            try
            {
                var product = _mapper.Map<Product>(productDto);

                // Check if the category exists
                if (productDto.CategoryId != 0)
                {
                    var categoryExists = await _context.Categories.AnyAsync(c => c.Id == productDto.CategoryId);
                    if (!categoryExists)
                    {
                        return new ServerResponse<ProductDto>(ResponseType.Error, "Specified category does not exist.", null);
                    }
                }

                await _context.Products.AddAsync(product);
                await _context.SaveChangesAsync();

                // Refresh the product to get the updated data including any database-generated values
                await _context.Entry(product).ReloadAsync();
                await _context.Entry(product).Reference(p => p.Category).LoadAsync();

                var productDtoResult = _mapper.Map<ProductDto>(product);

                _logger.LogTrace("Product added.");
                return new ServerResponse<ProductDto>(ResponseType.Success, "Product added.", productDtoResult);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "A database error occurred while adding the product.");
                return new ServerResponse<ProductDto>(ResponseType.Error, "A database error occurred while adding the product.", null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding the product.");
                return new ServerResponse<ProductDto>(ResponseType.Error, "An error occurred while adding the product.", null);
            }
        }

        public async Task<ServerResponse<ProductDto>> UpdateProductAsync(ProductDto productDto)
        {
            if (productDto == null)
            {
                return new ServerResponse<ProductDto>(ResponseType.Error, "Product data is null.", null);
            }

            if (productDto.Id <= 0)
            {
                return new ServerResponse<ProductDto>(ResponseType.Error, "Invalid product ID.", null);
            }

            if (string.IsNullOrWhiteSpace(productDto.Name))
            {
                return new ServerResponse<ProductDto>(ResponseType.Error, "Product name is required.", null);
            }

            if (productDto.Price < 0)
            {
                return new ServerResponse<ProductDto>(ResponseType.Error, "Product price cannot be negative.", null);
            }

            try
            {
                var product = await _context.Products.FindAsync(productDto.Id);
                if (product == null)
                {
                    return new ServerResponse<ProductDto>(ResponseType.Error, "Product not found.", null);
                }

                // Check if the category exists if it's being updated
                if (productDto.CategoryId != 0 && productDto.CategoryId != product.CategoryId)
                {
                    var categoryExists = await _context.Categories.AnyAsync(c => c.Id == productDto.CategoryId);
                    if (!categoryExists)
                    {
                        return new ServerResponse<ProductDto>(ResponseType.Error, "Specified category does not exist.", null);
                    }
                }

                var originalRowVersion = product.RowVersion;
                _mapper.Map(productDto, product);
                _context.Entry(product).OriginalValues["RowVersion"] = originalRowVersion;

                await _context.SaveChangesAsync();

                var productDtoResult = _mapper.Map<ProductDto>(product);

                _logger.LogTrace("Product updated.");
                return new ServerResponse<ProductDto>(ResponseType.Success, "Product updated.", productDtoResult);
            }
            catch (DbUpdateConcurrencyException)
            {
                _logger.LogWarning("Concurrency conflict while updating product.");
                return new ServerResponse<ProductDto>(ResponseType.Error, "A concurrency conflict occurred while updating the product. Please refresh and try again.", null);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "A database error occurred while updating the product.");
                return new ServerResponse<ProductDto>(ResponseType.Error, "A database error occurred while updating the product.", null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the product.");
                return new ServerResponse<ProductDto>(ResponseType.Error, "An error occurred while updating the product.", null);
            }
        }

        public async Task<ServerResponse<bool>> DeleteProductAsync(int id)
        {
            if (id <= 0)
            {
                return new ServerResponse<bool>(ResponseType.Error, "Invalid product ID.", false);
            }

            try
            {
                var product = await _context.Products.FindAsync(id);
                if (product == null)
                {
                    return new ServerResponse<bool>(ResponseType.Error, "Product not found.", false);
                }

                _context.Products.Remove(product);
                await _context.SaveChangesAsync();

                _logger.LogTrace("Product deleted.");
                return new ServerResponse<bool>(ResponseType.Success, "Product deleted.", true);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "A database error occurred while deleting the product.");
                return new ServerResponse<bool>(ResponseType.Error, "A database error occurred while deleting the product. It may be referenced by other entities.", false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the product.");
                return new ServerResponse<bool>(ResponseType.Error, "An error occurred while deleting the product.", false);
            }
        }
    }
}