using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs;
using Shared.Response;
using static Shared.Response.Enums;

namespace Application.Interfaces
{
    public interface IProductService
    {
        Task<ServicesResponse<List<ProductDto>>> GetProductsAsync();
        Task<ServicesResponse<ProductDto>> GetProductByIdAsync(int id);
        Task<ServicesResponse<ProductDto>> AddProductAsync(ProductDto productDto);
        Task<ServicesResponse<bool>> UpdateProductAsync(ProductDto productDto);
        Task<ServicesResponse<bool>> DeleteProductAsync(int id);
    }
}
