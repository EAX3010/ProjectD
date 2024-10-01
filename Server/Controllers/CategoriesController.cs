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
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ServerResponse<IEnumerable<CategoryDto>>), 200)]
        public async Task<ActionResult<ServerResponse<IEnumerable<CategoryDto>>>> GetCategories()
        {
            var result = await _categoryService.GetCategoriesAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ServerResponse<CategoryDto>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ServerResponse<CategoryDto>>> GetCategory(int id)
        {
            var result = await _categoryService.GetCategoryByIdAsync(id);
            if (result.Flag == Enums.ResponseType.Error)
                return NotFound(result);
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ServerResponse<CategoryDto>), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ServerResponse<CategoryDto>>> AddCategory([FromBody] CategoryDto categoryDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _categoryService.AddCategoryAsync(categoryDto);
            if (result.Flag == Enums.ResponseType.Success)
                return CreatedAtAction(nameof(GetCategory), new { id = result.Instance.Id }, result);
            return BadRequest(result);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ServerResponse<bool>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ServerResponse<bool>>> UpdateCategory(int id, [FromBody] CategoryDto categoryDto)
        {
            if (id != categoryDto.Id)
                return BadRequest(new ServerResponse<bool>(Enums.ResponseType.Error, "ID mismatch", false));
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _categoryService.UpdateCategoryAsync(categoryDto);
            if (result.Flag == Enums.ResponseType.Error)
                return NotFound(result);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ServerResponse<bool>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ServerResponse<bool>>> DeleteCategory(int id)
        {
            var result = await _categoryService.DeleteCategoryAsync(id);
            if (result.Flag == Enums.ResponseType.Error)
                return NotFound(result);
            return Ok(result);
        }
    }
}