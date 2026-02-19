using AutoMapper;
using FoodCorner.DTO;
using FoodCornerAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodCornerAPI.Controllers
{
    [ApiController]
    [Route("api/food")]
    public class FoodController : ControllerBase
    {
        private readonly FoodCornerDbContext _context;
        private readonly IMapper _mapper;

        public FoodController(FoodCornerDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetFoods()
        {
            var foods = await _context.FoodItems.Include(f => f.Category).ToListAsync();
            return Ok(_mapper.Map<List<FoodDTO>>(foods));
        }

        [HttpPost]
        public async Task<IActionResult> CreateFood(FoodItemCreateDTO dto)
        {
            var categoryExists = await _context.Categories.AnyAsync(c => c.CategoryId == dto.CategoryId);
            
            if (!categoryExists)
                return BadRequest("Invalid CategoryId");
            var food = _mapper.Map<FoodItem>(dto);
            _context.FoodItems.Add(food);
            await _context.SaveChangesAsync();
            await _context.Entry(food).Reference(f => f.Category).LoadAsync();
            return Ok(_mapper.Map<FoodDTO>(food));
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateFood(int id, FoodItemUpdateDTO dto)
        {
            if (id != dto.FoodItemId)
                return BadRequest("ID mismatch");
            var food = await _context.FoodItems.FindAsync(id);
            if (food == null)
                return NotFound("Food not found");
            _mapper.Map(dto, food);
            await _context.SaveChangesAsync();
            await _context.Entry(food).Reference(f => f.Category).LoadAsync(); 
            return Ok(_mapper.Map<FoodDTO>(food));
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteFood(int id)
        {
            var food = await _context.FoodItems.FindAsync(id);
            if (food == null)
                return NotFound("Food not found");

            _context.FoodItems.Remove(food);
            await _context.SaveChangesAsync();

            return Ok("Food deleted successfully");
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetFoodById(int id)
        {
            var food = await _context.FoodItems.Include(f => f.Category).FirstOrDefaultAsync(f => f.FoodItemId == id);
            if (food == null)
                return NotFound();

            return Ok(_mapper.Map<FoodDTO>(food));
        }

        [HttpGet("category-wise")]
        public async Task<IActionResult> GetCategoryWiseMenu()
        {
            var data = await _context.Categories.Include(c => c.FoodItems).Select(c => new
                {
                    CategoryId = c.CategoryId,
                    CategoryName = c.CategoryName,
                    Foods = c.FoodItems.Select(f => new
                    {
                        f.FoodItemId,
                        f.FoodItemName,
                        f.Price,
                        f.Description,
                        f.Image
                    }).ToList()
                })
                .ToListAsync();

            return Ok(data);
        }

    }
}
