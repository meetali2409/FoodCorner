using FoodCornerAPI.DTO;

namespace FoodCorner.DTO
{
    public class OrderDTO
    {
        public int OrderId { get; set; }
        public string CustomerName { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
        public List<OrderItemResponseDTO> Items { get; set; } = new List<OrderItemResponseDTO>();
    }
}
