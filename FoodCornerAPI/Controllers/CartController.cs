using FoodCornerAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodCornerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly FoodCornerDbContext _context;

        public CartController(FoodCornerDbContext context)
        {
            _context = context;
        }

        [HttpPost("add")]
        public IActionResult Add(int foodId, string userId)
        {
            var item = _context.Carts
                .FirstOrDefault(x => x.FoodItemId == foodId && x.UserId == userId);

            if (item != null)
            {
                item.Quantity++;
            }
            else
            {
                _context.Carts.Add(new Cart
                {
                    FoodItemId = foodId,
                    UserId = userId,
                    Quantity = 1
                });
            }

            _context.SaveChanges();
            return Ok();
        }
        [HttpGet("{userId}")]
        public IActionResult GetCart(string userId)
        {
            var items = _context.Carts.Include(x => x.FoodItem).Where(x => x.UserId == userId).Select(x => new
            {
                x.FoodItemId,
                x.Quantity,
                FoodItemName = x.FoodItem.FoodItemName,
                Price = x.FoodItem.Price,
                Image = x.FoodItem.Image
            })
            .ToList();
      

            return Ok(items);
        }
        

        [HttpPost("increase")]
        public IActionResult Increase(int foodId, string userId)
        {
            var item = _context.Carts
                .FirstOrDefault(x => x.FoodItemId == foodId && x.UserId == userId);

            if (item != null)
            {
                item.Quantity++;
                _context.SaveChanges();
            }

            return Ok();
        }

        [HttpPost("decrease")]
        public IActionResult Decrease(int foodId, string userId)
        {
            var item = _context.Carts
                .FirstOrDefault(x => x.FoodItemId == foodId && x.UserId == userId);

            if (item != null)
            {
                item.Quantity--;

                if (item.Quantity <= 0)
                {
                    _context.Carts.Remove(item);
                }

                _context.SaveChanges();
            }

            return Ok();
        }

        [HttpDelete("remove")]
        public IActionResult Remove(int foodId, string userId)
        {
            var item = _context.Carts
                .FirstOrDefault(x => x.FoodItemId == foodId && x.UserId == userId);

            if (item != null)
            {
                _context.Carts.Remove(item);
                _context.SaveChanges();
            }

            return Ok();
        }
        
        [HttpDelete("clear/{userId}")]
        public IActionResult ClearCart(string userId)
        {
            var items = _context.Carts
                .Where(x => x.UserId == userId)
                .ToList();

            _context.Carts.RemoveRange(items);
            _context.SaveChanges();

            return Ok();
        }
    }
}
