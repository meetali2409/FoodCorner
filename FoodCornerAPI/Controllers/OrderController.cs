using AutoMapper;
using FoodCornerAPI.Data;
using FoodCornerAPI.Models;
using Microsoft.AspNetCore.Mvc;
using FoodCorner.DTO;
using Microsoft.EntityFrameworkCore;


namespace FoodCornerAPI.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrderController : ControllerBase
    {
        private readonly FoodCornerDbContext _context;
        private readonly IMapper _mapper;

        public OrderController(FoodCornerDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<OrderResponseDTO>>>> GetOrders()
        {
            var orders = await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.FoodItem) 
                .ToListAsync();

            var result = _mapper.Map<List<OrderResponseDTO>>(orders);

            return Ok(ApiResponse<List<OrderResponseDTO>>.Ok(
                result, "Orders fetched successfully"));
        }

    
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<OrderResponseDTO>>> GetOrder(int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.FoodItem) 
                .FirstOrDefaultAsync(o => o.OrderId == id);

            if (order == null)
                return NotFound(ApiResponse<OrderResponseDTO>
                    .Notfound("Order not found"));

            var result = _mapper.Map<OrderResponseDTO>(order);

            return Ok(ApiResponse<OrderResponseDTO>
                  .Ok(result, "Order fetched successfully"));

        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<OrderResponseDTO>>> CreateOrder(OrderCreateDTO dto)
        {
            if (dto.Items == null || !dto.Items.Any())
                return BadRequest(ApiResponse<OrderResponseDTO>
                    .Error(400, "Order must contain at least one item"));

            var order = _mapper.Map<Order>(dto);
            order.OrderDate = DateTime.Now;
            order.OrderItems = new List<OrderItem>();

            decimal totalAmount = 0;

            foreach (var itemDto in dto.Items)
            {
                var foodItem = await _context.FoodItems
                    .FirstOrDefaultAsync(f => f.FoodItemId == itemDto.FoodItemId);

                if (foodItem == null)
                    return BadRequest(ApiResponse<OrderResponseDTO>
                        .Error(400, $"Food item not found: {itemDto.FoodItemId}"));

                var orderItem = _mapper.Map<OrderItem>(itemDto);

                orderItem.FoodItemId = foodItem.FoodItemId;
                orderItem.UnitPrice = foodItem.Price;
               
                totalAmount += orderItem.UnitPrice * orderItem.Quantity;

                order.OrderItems.Add(orderItem);
            }

            order.TotalAmount = totalAmount;

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            var savedOrder = await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.FoodItem)
                .FirstOrDefaultAsync(o => o.OrderId == order.OrderId);

            var result = _mapper.Map<OrderResponseDTO>(savedOrder);
            var cartItems = _context.Carts.Where(x => x.UserId == dto.UserId).ToList();

            _context.Carts.RemoveRange(cartItems);
            await _context.SaveChangesAsync();

            return Ok(ApiResponse<OrderResponseDTO>
                .Ok(result, "Order created successfully"));
        }

        [HttpPut("{id}/status")]
        public async Task<ActionResult<ApiResponse<OrderResponseDTO>>> UpdateStatus(int id, OrderStatusUpdateDTO dto)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.FoodItem)
                .FirstOrDefaultAsync(o => o.OrderId == id);

            if (order == null)
                return NotFound(ApiResponse<OrderResponseDTO>
                    .Notfound("Order not found"));

            order.Status = dto.Status;

            await _context.SaveChangesAsync();

            var result = _mapper.Map<OrderResponseDTO>(order);

            return Ok(ApiResponse<OrderResponseDTO>
                .Ok(result, "Order status updated successfully"));
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteOrder(int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.OrderId == id);

            if (order == null)
                return NotFound(ApiResponse<object>
                    .Notfound("Order not found"));

            _context.OrderItems.RemoveRange(order.OrderItems);
            _context.Orders.Remove(order);

            await _context.SaveChangesAsync();

            return Ok(ApiResponse<object>
                .NoContent("Order deleted successfully"));
        }

    }
}
