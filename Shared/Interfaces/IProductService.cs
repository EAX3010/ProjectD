using System.Collections.Generic;
using System.Threading.Tasks;
using Shared.DTOs;
using Shared.Response;
using static Shared.Response.Enums;

namespace Application.Interfaces
{
    public interface IProductService
    {
        Task<ServerResponse<IEnumerable<ProductDto>>> GetProductsAsync();
        Task<ServerResponse<ProductDto>> GetProductByIdAsync(int id);
        Task<ServerResponse<ProductDto>> AddProductAsync(ProductDto productDto);
        Task<ServerResponse<ProductDto>> UpdateProductAsync(ProductDto productDto);
        Task<ServerResponse<bool>> DeleteProductAsync(int id);
    }
}
