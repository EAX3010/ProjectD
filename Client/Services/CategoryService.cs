using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Application.Interfaces;
using Shared.DTOs;
using Shared.Response;

namespace Client.Services
{
    public class CategoryService(HttpClient _httpClient) : ICategoryService
    {
        private const string _baseUrl = "api/categories";


        public async Task<ServerResponse<IEnumerable<CategoryDto>>> GetCategoriesAsync()
        {
            var response = await _httpClient.GetAsync(_baseUrl);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ServerResponse<IEnumerable<CategoryDto>>>();
                return result ?? new ServerResponse<IEnumerable<CategoryDto>>(Enums.ResponseType.Error, "Error deserializing response");
            }
            return new ServerResponse<IEnumerable<CategoryDto>>(Enums.ResponseType.Error, "Error fetching categories");
        }

        public async Task<ServerResponse<CategoryDto>> GetCategoryByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/{id}");
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ServerResponse<CategoryDto>>();
                return result ?? new ServerResponse<CategoryDto>(Enums.ResponseType.Error, "Error deserializing response");
            }
            return new ServerResponse<CategoryDto>(Enums.ResponseType.Error, "Error fetching category");
        }

        public async Task<ServerResponse<CategoryDto>> AddCategoryAsync(CategoryDto categoryDto)
        {
            var response = await _httpClient.PostAsJsonAsync(_baseUrl, categoryDto);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ServerResponse<CategoryDto>>();
                return result ?? new ServerResponse<CategoryDto>(Enums.ResponseType.Error, "Error deserializing response");
            }
            return new ServerResponse<CategoryDto>(Enums.ResponseType.Error, "Failed to add category");
        }

        public async Task<ServerResponse<bool>> UpdateCategoryAsync(CategoryDto categoryDto)
        {
            var response = await _httpClient.PutAsJsonAsync($"{_baseUrl}/{categoryDto.Id}", categoryDto);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ServerResponse<bool>>();
                return result ?? new ServerResponse<bool>(Enums.ResponseType.Error, "Error deserializing response");
            }
            return new ServerResponse<bool>(Enums.ResponseType.Error, "Failed to update category");
        }

        public async Task<ServerResponse<bool>> DeleteCategoryAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{_baseUrl}/{id}");
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ServerResponse<bool>>();
                return result ?? new ServerResponse<bool>(Enums.ResponseType.Error, "Error deserializing response");
            }
            return new ServerResponse<bool>(Enums.ResponseType.Error, "Failed to delete category");
        }
    }
}