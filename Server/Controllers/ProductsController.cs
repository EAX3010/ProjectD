using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;
using Shared.Response;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        // Inject the IProductService
        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        /// <summary>
        /// Get all products
        /// </summary>
        /// <returns>A list of all products</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ProductDto>), 200)]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts()
        {
            var products = await _productService.GetProductsAsync();
            return Ok(products);
        }

        /// <summary>
        /// Get a specific product by id
        /// </summary>
        /// <param name="id">The id of the product to retrieve</param>
        /// <returns>The requested product</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProductDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ProductDto>> GetProduct(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
                return NotFound();
            return Ok(product);
        }

        /// <summary>
        /// Create a new product
        /// </summary>
        /// <param name="productDto">The product to create</param>
        /// <returns>The created product</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ProductDto), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ProductDto>> AddProduct([FromBody] ProductDto productDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            ServicesResponse<ProductDto> result = await _productService.AddProductAsync(productDto);
            if (result.Flag)
                return CreatedAtAction(nameof(GetProduct), new { id = result.Instance.Id }, result.Instance);

            return BadRequest(result.Message);
        }

        /// <summary>
        /// Update an existing product
        /// </summary>
        /// <param name="id">The id of the product to update</param>
        /// <param name="productDto">The updated product data</param>
        /// <returns>No content</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(409)]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductDto productDto)
        {
            if (id != productDto.Id)
                return BadRequest("ID mismatch");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _productService.UpdateProductAsync(productDto);
            if (!result)
            {
                return Conflict("The product was modified by another user. Please reload and try again.");
            }
            return NoContent();
        }

        /// <summary>
        /// Delete a specific product
        /// </summary>
        /// <param name="id">The id of the product to delete</param>
        /// <returns>No content</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var result = await _productService.DeleteProductAsync(id);
            if (!result)
                return NotFound();
            return NoContent();
        }
    }
}
