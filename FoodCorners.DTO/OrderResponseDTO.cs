namespace FoodCorner.DTO
{
    public class OrderResponseDTO
    {
        public int OrderId { get; set; }
        public string CustomerName { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }

        public List<OrderItemResponseDTO> Items { get; set; }
    }
}
