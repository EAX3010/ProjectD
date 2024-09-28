using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs;

namespace Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CategoryService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // Get all categories
        public async Task<IEnumerable<CategoryDto>> GetCategoriesAsync()
        {
            return await _context.Categories
                .ProjectTo<CategoryDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        // Get category by ID
        public async Task<CategoryDto?> GetCategoryByIdAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            return _mapper.Map<CategoryDto>(category);
        }

        // Add a new category
        public async Task<CategoryDto> AddCategoryAsync(CategoryDto categoryDto)
        {
            var category = _mapper.Map<Category>(categoryDto);

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return _mapper.Map<CategoryDto>(category);
        }

        // Update an existing category
        public async Task<bool> UpdateCategoryAsync(CategoryDto categoryDto)
        {
            var category = await _context.Categories.FindAsync(categoryDto.Id);
            if (category == null)
                return false;

            // Copy the original RowVersion
            var originalRowVersion = category.RowVersion;

            _mapper.Map(categoryDto, category);

            // Set the original RowVersion
            _context.Entry(category).OriginalValues["RowVersion"] = originalRowVersion;

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

        // Delete a category
        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return false;

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
