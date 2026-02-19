using AutoMapper;
using FoodCornerAPI.Models;
using FoodCorner.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodCornerAPI.Controllers
{
    [ApiController]
    [Route("api/category")]
    public class CategoryController : ControllerBase
    {
        private readonly FoodCornerDbContext _context;
        private readonly IMapper _mapper;

        public CategoryController(FoodCornerDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _context.Categories.ToListAsync();
            return Ok(_mapper.Map<List<CategoryDTO>>(categories));
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory(CategoryCreateDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.CategoryName))
                return BadRequest("Category name is required");

            var exists = await _context.Categories
                .AnyAsync(c => c.CategoryName.ToLower() == dto.CategoryName.ToLower());

            if (exists)
                return Conflict("Category already exists");

            var category = _mapper.Map<Category>(dto);
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return Ok(_mapper.Map<CategoryDTO>(category));
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateCategory(int id, CategoryCreateDTO dto)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return NotFound("Category not found");

            category.CategoryName = dto.CategoryName;
            await _context.SaveChangesAsync();

            return Ok(_mapper.Map<CategoryDTO>(category));
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.Categories
                .Include(c => c.FoodItems)
                .FirstOrDefaultAsync(c => c.CategoryId == id);

            if (category == null)
                return NotFound("Category not found");

            if (category.FoodItems.Any())
                return BadRequest("Cannot delete category. Food items exist.");

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return Ok("Category deleted successfully");
        }
    }
}
