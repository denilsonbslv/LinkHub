using LinkHub.Api.Dtos;
using LinkHub.Api.Infrastructure.Data;
using LinkHub.Api.Services;
using LinkHub.Api.Services.Interfaces;
using LinkHub.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LinkHub.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CategoriesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ICategoryService _categoryService;

        public CategoriesController(ApplicationDbContext context, ICategoryService categoryService)
        {
            _context = context;
            _categoryService = categoryService;
        }

        /// <summary>
        /// Obtém uma categoria pelo seu ID, garantindo que pertence ao usuário autenticado.
        /// </summary>
        /// <response code="200">Retorna a categoria solicitada.</response>
        /// <response code="401">Se o usuário não estiver autenticado.</response>
        /// <response code="404">Se a categoria não for encontrada ou não pertencer ao usuário.</response>
        /// <returns>Um DTO da entidade category.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CategoryDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var category = await _categoryService.GetByIdAsync(id, userId);

            if (category == null)
            {
                return NotFound("Categoria não encontrada.");
            }

            return Ok(category);
        }

        /// <summary>
        /// Cria uma nova categoria para o usuário autenticado.
        /// </summary>
        /// <param name="createCategoryDto">Os dados da nova categoria.</param>
        /// <response code="201">Retorna a categoria recem-criada.</response>
        /// <response code="400">Se os dados forem inválidos.</response>
        /// <response code="401">Se o usuário não estiver autenticado.</response>
        [HttpPost]
        [ProducesResponseType(typeof(Category), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryDto createCategoryDto)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var existingCategory = _context.Categories.FirstOrDefault(c => c.Name == createCategoryDto.Name && c.UserId == userId);

            if (existingCategory != null)
            {
                return BadRequest("Já existe uma categoria com esse nome para o usuário.");
            }

            var newCategory = await _context.Categories.AddAsync(new Category
            {
                Name = createCategoryDto.Name,
                Color = createCategoryDto.Color,
                UserId = userId
            });
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCategoryById), new { id = newCategory.Entity.Id }, newCategory.Entity);
        }

        /// <summary>
        /// Obtém todas as categorias do usuário autenticado.
        /// </summary>
        /// <response code="200">Retorna a lista de categorias do usuário.</response>
        /// <response code="401">Se o usuário não estiver autenticado.</response>
        /// <response code="404">Se nenhuma categoria for encontrada para o usuário.</response>
        /// <returns>Um DTO da entidade category.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<CategoryDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAllCategories()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var categories = _context.Categories.Where(c => c.UserId == userId).ToList();

            if (categories.Count == 0)
            {
                return NotFound("Nenhuma categoria encontrada para o usuário.");
            }

            return Ok(categories);
        }

        /// <summary>
        /// Atualiza uma categoria existente do usuário autenticado.
        /// </summary>
        /// <response code="204">Se a categoria for atualizada com sucesso.</response>
        /// <response code="400">Se os dados forem inválidos.</response>
        /// <response code="401">Se o usuário não estiver autenticado.</response>
        /// <returns>Um DTO com a category atualizada</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryDto updateCategoryDto)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var category = await _context.Categories.FindAsync(id);
            if (category == null || category.UserId != userId)
            {
                return NotFound("Categoria não encontrada.");
            }
            category.Name = updateCategoryDto.Name;
            category.Color = updateCategoryDto.Color;
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Deleta logicamente uma categoria do usuário autenticado.
        /// </summary>
        /// <response code="204">Se a categoria for deletada com sucesso.</response>
        /// <response code="401">Se o usuário não estiver autenticado.</response>
        /// <response code="404">Se a categoria não for encontrada ou não pertencer ao usuário.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var category = await _context.Categories.FindAsync(id);
            if (category == null || category.UserId != userId)
            {
                return NotFound("Categoria não encontrada.");
            }
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
