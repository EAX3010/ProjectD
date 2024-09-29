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

        public async Task<ServicesResponse<List<ProductDto>>> GetProductsAsync()
        {
            try
            {
                var products = await _context.Products
                    .ProjectTo<ProductDto>(_mapper.ConfigurationProvider)
                    .ToListAsync();

                _logger.LogTrace("Products retrieved.");
                return new ServicesResponse<List<ProductDto>>(ResponseType.Success, "Products retrieved.", products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving products.");
                return new ServicesResponse<List<ProductDto>>(ResponseType.Error, "An error occurred while retrieving products.", null);
            }
        }

        public async Task<ServicesResponse<ProductDto>> GetProductByIdAsync(int id)
        {
            try
            {
                var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
                if (product == null)
                    return new ServicesResponse<ProductDto>(ResponseType.Error, "Product not found.", null);

                _logger.LogTrace("Product retrieved.");
                return new ServicesResponse<ProductDto>(ResponseType.Success, "Product retrieved.", _mapper.Map<ProductDto>(product));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the product.");
                return new ServicesResponse<ProductDto>(ResponseType.Error, "An error occurred while retrieving the product.", null);
            }
        }

        public async Task<ServicesResponse<ProductDto>> AddProductAsync(ProductDto productDto)
        {
            if (productDto == null)
                return new ServicesResponse<ProductDto>(ResponseType.Error, "Product data is null.", null);

            try
            {
                var product = _mapper.Map<Product>(productDto);
                await _context.Products.AddAsync(product);
                await _context.SaveChangesAsync();
                await _context.Entry(product).Reference(p => p.Category).LoadAsync();
                var productDtoResult = _mapper.Map<ProductDto>(product);

                _logger.LogTrace("Product added.");
                return new ServicesResponse<ProductDto>(ResponseType.Success, "Product added.", productDtoResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding the product.");
                return new ServicesResponse<ProductDto>(ResponseType.Error, "An error occurred while adding the product.", null);
            }
        }

        public async Task<ServicesResponse<bool>> UpdateProductAsync(ProductDto productDto)
        {
            if (productDto == null)
                return new ServicesResponse<bool>(ResponseType.Error, "Product data is null.", false);

            try
            {
                var product = await _context.Products.FindAsync(productDto.Id);
                if (product == null)
                    return new ServicesResponse<bool>(ResponseType.Error, "Product not found.", false);

                var originalRowVersion = product.RowVersion;
                _mapper.Map(productDto, product);
                _context.Entry(product).OriginalValues["RowVersion"] = originalRowVersion;
                await _context.SaveChangesAsync();

                _logger.LogTrace("Product updated.");
                return new ServicesResponse<bool>(ResponseType.Success, "Product updated.", true);
            }
            catch (DbUpdateConcurrencyException)
            {
                _logger.LogWarning("Concurrency conflict while updating product.");
                return new ServicesResponse<bool>(ResponseType.Error, "A concurrency conflict occurred while updating the product.", false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the product.");
                return new ServicesResponse<bool>(ResponseType.Error, "An error occurred while updating the product.", false);
            }
        }

        public async Task<ServicesResponse<bool>> DeleteProductAsync(int id)
        {
            try
            {
                var product = await _context.Products.FindAsync(id);
                if (product == null)
                    return new ServicesResponse<bool>(ResponseType.Error, "Product not found.", false);

                _context.Products.Remove(product);
                await _context.SaveChangesAsync();

                _logger.LogTrace("Product deleted.");
                return new ServicesResponse<bool>(ResponseType.Success, "Product deleted.", true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the product.");
                return new ServicesResponse<bool>(ResponseType.Error, "An error occurred while deleting the product.", false);
            }
        }
    }
}