using AutoMapper;
using FoodCorner.DTO;
using FoodCornerAPI.Data;
using FoodCornerAPI.DTO;
using FoodCornerAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodCornerAPI.Controllers
{
    [ApiController]
    [Route("api/orderitem/{orderId}")]
    public class OrderItemController : ControllerBase
    {
        private readonly FoodCornerDbContext _context;
        private readonly IMapper _mapper;

        public OrderItemController(FoodCornerDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddToCart(int orderId, OrderItemCreateDTO dto)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null) return NotFound("Order not found");

            var food = await _context.FoodItems
                .FirstOrDefaultAsync(f => f.FoodItemId == dto.FoodItemId);
            if (food == null) return NotFound("Food item not found");

            var existingItem = await _context.OrderItems
                .FirstOrDefaultAsync(i =>
                    i.OrderId == orderId && i.FoodItemId == dto.FoodItemId);

            if (existingItem != null)
            {
                existingItem.Quantity += dto.Quantity;
            }
            else
            {
                var item = new OrderItem
                {
                    OrderId = orderId,
                    FoodItemId = food.FoodItemId,
                    Quantity = dto.Quantity,
                    UnitPrice = food.Price
                };
                _context.OrderItems.Add(item);
            }

            await _context.SaveChangesAsync();
            return Ok("Item added");
        }

        [HttpGet]
        public async Task<IActionResult> ViewCart(int orderId)
        {
            var items = await _context.OrderItems
                .Where(i => i.OrderId == orderId)
                .Include(i => i.FoodItem)
                .ToListAsync();

            var result = _mapper.Map<List<OrderItemResponseDTO>>(items); 

            var totalItems = result.Sum(i => i.Quantity);

            return Ok(new
            {
                orderId = orderId,
                totalSelectedItems = totalItems,
                items = result
            });
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateCart(int orderId,[FromBody] CartUpdateDTO dto)
        {
            var item = await _context.OrderItems
                .FirstOrDefaultAsync(i =>
                    i.OrderId == orderId &&
                    i.FoodItemId == dto.FoodItemId);

            if (item == null)
                return NotFound("Item not found");

            item.Quantity = dto.Quantity;
            await _context.SaveChangesAsync();

            return Ok("Cart updated");
        }

        [HttpPut("exchange")]
        public async Task<IActionResult> ExchangeItem(int orderId,[FromBody] ExchangeItemDTO dto)
        {
            var item = await _context.OrderItems
                .FirstOrDefaultAsync(i =>
                    i.OrderId == orderId &&
                    i.FoodItemId == dto.OldFoodItemId);

            if (item == null)
                return NotFound("Item not found");

            var food = await _context.FoodItems
                .FirstOrDefaultAsync(f => f.FoodItemId == dto.NewFoodItemId);

            if (food == null)
                return NotFound("New food item not found");

            item.FoodItemId = food.FoodItemId;
            item.UnitPrice = food.Price;
            item.Quantity = dto.Quantity;

            await _context.SaveChangesAsync();

            return Ok("Item exchanged");
        }

        [HttpDelete("remove/{itemId}")]
        public async Task<IActionResult> RemoveFromCart(int orderId, int itemId)
        {
            var item = await _context.OrderItems
                .FirstOrDefaultAsync(i =>
                    i.OrderItemId == itemId && i.OrderId == orderId);

            if (item == null) return NotFound("Item not found");

            _context.OrderItems.Remove(item);
            await _context.SaveChangesAsync();

            return Ok("Item removed");
        }
    }
}
