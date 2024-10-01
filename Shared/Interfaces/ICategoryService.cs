using System.Collections.Generic;
using System.Threading.Tasks;
using Shared.DTOs;
using Shared.Response;
using static Shared.Response.Enums;

namespace Application.Interfaces
{
    public interface ICategoryService
    {
        Task<ServerResponse<IEnumerable<CategoryDto>>> GetCategoriesAsync();

        Task<ServerResponse<CategoryDto>> GetCategoryByIdAsync(int id);

        Task<ServerResponse<CategoryDto>> AddCategoryAsync(CategoryDto categoryDto);

        Task<ServerResponse<bool>> UpdateCategoryAsync(CategoryDto categoryDto);

        Task<ServerResponse<bool>> DeleteCategoryAsync(int id);
    }
}
