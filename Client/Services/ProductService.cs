using Application.Interfaces;
using Shared.DTOs;
using Shared.Response;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Client.Services
{
    public class ProductService(HttpClient _httpClient) : IProductService
    {
        private const string _baseUrl = "api/products";

        

        public async Task<ServerResponse<IEnumerable<ProductDto>>> GetProductsAsync()
        {
            var response = await _httpClient.GetAsync(_baseUrl);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ServerResponse<IEnumerable<ProductDto>>>();
                return result ?? new ServerResponse<IEnumerable<ProductDto>>(Enums.ResponseType.Error, "Error deserializing response");
            }
            return new ServerResponse<IEnumerable<ProductDto>>(Enums.ResponseType.Error, "Error fetching products");
        }

        public async Task<ServerResponse<ProductDto>> GetProductByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/{id}");
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ServerResponse<ProductDto>>();
                return result ?? new ServerResponse<ProductDto>(Enums.ResponseType.Error, "Error deserializing response");
            }
            return new ServerResponse<ProductDto>(Enums.ResponseType.Error, "Error fetching product");
        }

        public async Task<ServerResponse<ProductDto>> AddProductAsync(ProductDto productDto)
        {
            try
            {
                
                var response = await _httpClient.PostAsJsonAsync(_baseUrl, productDto);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<ServerResponse<ProductDto>>();
                    return result ?? new ServerResponse<ProductDto>(Enums.ResponseType.Error, "Error deserializing response");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    var errorResult = await response.Content.ReadFromJsonAsync<ServerResponse<ProductDto>>();
                    return errorResult ?? new ServerResponse<ProductDto>(Enums.ResponseType.Error, "Bad request");
                }
                else
                {
                    return new ServerResponse<ProductDto>(Enums.ResponseType.Error, $"HTTP error: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                return new ServerResponse<ProductDto>(Enums.ResponseType.Error, $"Exception: {ex.Message}");
            }
        }

        public async Task<ServerResponse<ProductDto>> UpdateProductAsync(ProductDto productDto)
        {
            var response = await _httpClient.PutAsJsonAsync($"{_baseUrl}/{productDto.Id}", productDto);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ServerResponse<ProductDto>>();
                return result ?? new ServerResponse<ProductDto>(Enums.ResponseType.Error, "Error deserializing response");
            }
            return new ServerResponse<ProductDto>(Enums.ResponseType.Error, "Failed to update product");
        }

        public async Task<ServerResponse<bool>> DeleteProductAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{_baseUrl}/{id}");
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ServerResponse<bool>>();
                return result ?? new ServerResponse<bool>(Enums.ResponseType.Error, "Error deserializing response");
            }
            return new ServerResponse<bool>(Enums.ResponseType.Error, "Failed to delete product");
        }
    }
}