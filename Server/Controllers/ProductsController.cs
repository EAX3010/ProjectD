using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;
using Shared.Response;
using static Shared.Response.Enums;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ServerResponse<IEnumerable<ProductDto>>), 200)]
        public async Task<ActionResult<ServerResponse<IEnumerable<ProductDto>>>> GetProducts()
        {
            var result = await _productService.GetProductsAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ServerResponse<ProductDto>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ServerResponse<ProductDto>>> GetProduct(int id)
        {
            var result = await _productService.GetProductByIdAsync(id);
            if (result.Flag == Enums.ResponseType.Error)
                return NotFound(result);

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ServerResponse<ProductDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ServerResponse<ProductDto>>> AddProduct([FromBody] ProductDto productDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ServerResponse<ProductDto>(ResponseType.Error, "Invalid model state"));
            }

            try
            {
                var result = await _productService.AddProductAsync(productDto);

                if (result.Flag == ResponseType.Success)
                {
                    return CreatedAtAction(nameof(GetProduct), new { id = result.Instance.Id }, result);
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ServerResponse<ProductDto>(ResponseType.Error, "An unexpected error occurred. Please try again later."));
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ServerResponse<ProductDto>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ServerResponse<ProductDto>>> UpdateProduct(int id, [FromBody] ProductDto productDto)
        {
            if (id != productDto.Id)
                return BadRequest(new ServerResponse<ProductDto>(Enums.ResponseType.Error, "ID mismatch", null));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _productService.UpdateProductAsync(productDto);
            if (result.Flag == Enums.ResponseType.Error)
                return NotFound(result);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ServerResponse<bool>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ServerResponse<bool>>> DeleteProduct(int id)
        {
            var result = await _productService.DeleteProductAsync(id);
            if (result.Flag == Enums.ResponseType.Error)
                return NotFound(result);

            return Ok(result);
        }
    }
}