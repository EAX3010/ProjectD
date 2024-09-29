using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs;
using Shared.Response;
using static Shared.Response.Enums;

namespace Application.Interfaces
{
    public interface ICategoryService
    {
        Task<ServicesResponse<List<CategoryDto>>> GetCategoriesAsync();

        Task<ServicesResponse<CategoryDto>> GetCategoryByIdAsync(int id);

        Task<ServicesResponse<CategoryDto>> AddCategoryAsync(CategoryDto categoryDto);

        Task<ServicesResponse<bool>> UpdateCategoryAsync(CategoryDto categoryDto);

        Task<ServicesResponse<bool>> DeleteCategoryAsync(int id);
    }
}
