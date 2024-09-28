using System.Collections.Generic;
using System.Threading.Tasks;
using Shared.DTOs;
using Shared.Response;

namespace Application.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetProductsAsync();
        Task<ProductDto?> GetProductByIdAsync(int id);
        Task<ServicesResponse<ProductDto>> AddProductAsync(ProductDto productDto);
        Task<bool> UpdateProductAsync(ProductDto productDto);
        Task<bool> DeleteProductAsync(int id);
    }
}
