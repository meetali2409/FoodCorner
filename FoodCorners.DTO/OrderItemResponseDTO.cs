namespace FoodCorner.DTO
{
    public class OrderItemResponseDTO
    {
        public int OrderItemId { get; set; }

        public int FoodItemId { get; set; }
        public string FoodItemName { get; set; } = string.Empty;

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }
}