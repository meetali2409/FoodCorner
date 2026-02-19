
namespace FoodCorner.DTO
{
    public class OrderCreateDTO
    {
        public string CustomerName { get; set; }
        public string UserId { get; set; }

        public List<OrderItemCreateDTO> Items { get; set; } = new();
    }
}