using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

namespace Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<CategoryService> _logger;

        public CategoryService(AppDbContext context, IMapper mapper, ILogger<CategoryService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ServicesResponse<List<CategoryDto>>> GetCategoriesAsync()
        {
            try
            {
                var categories = await _context.Categories
                    .ProjectTo<CategoryDto>(_mapper.ConfigurationProvider)
                    .ToListAsync();

                _logger.LogTrace("Categories found.");
                return new ServicesResponse<List<CategoryDto>>(ResponseType.Success, "Categories found.", categories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.InnerException?.Message);
                return new ServicesResponse<List<CategoryDto>>(ResponseType.Error, "An error occurred while retrieving categories.", null);
            }
        }

        public async Task<ServicesResponse<CategoryDto>> GetCategoryByIdAsync(int id)
        {
            try
            {
                var category = await _context.Categories.FindAsync(id);
                if (category == null)
                    return new ServicesResponse<CategoryDto>(ResponseType.Error, "Category not found.", null);

                _logger.LogTrace("Category found.");
                return new ServicesResponse<CategoryDto>(ResponseType.Success, "Category found.", _mapper.Map<CategoryDto>(category));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.InnerException?.Message);
                return new ServicesResponse<CategoryDto>(ResponseType.Error, "An error occurred while retrieving the category.", null);
            }
        }

        public async Task<ServicesResponse<CategoryDto>> AddCategoryAsync(CategoryDto categoryDto)
        {
            if (categoryDto == null)
                return new ServicesResponse<CategoryDto>(ResponseType.Error, "Category is null", null);

            var category = _mapper.Map<Category>(categoryDto);
            try
            {
                await _context.Categories.AddAsync(category);
                await _context.SaveChangesAsync();
                var categoryDtoResult = _mapper.Map<CategoryDto>(category);
                _logger.LogTrace("Category Added.");
                return new ServicesResponse<CategoryDto>(ResponseType.Success, "Category Added.", categoryDtoResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.InnerException?.Message);
                return new ServicesResponse<CategoryDto>(ResponseType.Error, "An error occurred while adding the category.", null);
            }
        }

        public async Task<ServicesResponse<bool>> UpdateCategoryAsync(CategoryDto categoryDto)
        {
            var category = await _context.Categories.FindAsync(categoryDto.Id);
            if (category == null)
            {
                return new ServicesResponse<bool>(ResponseType.Error, "Category not found", false);
            }

            try
            {
                var originalRowVersion = category.RowVersion;
                _mapper.Map(categoryDto, category);
                _context.Entry(category).OriginalValues["RowVersion"] = originalRowVersion;
                await _context.SaveChangesAsync();
                _logger.LogTrace("Category Updated.");
                return new ServicesResponse<bool>(ResponseType.Success, "Category Updated.", true);
            }
            catch (DbUpdateConcurrencyException)
            {
                _logger.LogWarning("Concurrency conflict while updating category.");
                return new ServicesResponse<bool>(ResponseType.Warning, "A concurrency conflict occurred while updating the category.", false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.InnerException?.Message);
                return new ServicesResponse<bool>(ResponseType.Error, "An error occurred while updating the category.", false);
            }
        }

        public async Task<ServicesResponse<bool>> DeleteCategoryAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return new ServicesResponse<bool>(ResponseType.Error, "Category not found", false);

            try
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
                _logger.LogTrace("Category Deleted.");
                return new ServicesResponse<bool>(ResponseType.Success, "Category Deleted.", true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.InnerException?.Message);
                return new ServicesResponse<bool>(ResponseType.Error, "An error occurred while deleting the category.", false);
            }
        }
    }
}