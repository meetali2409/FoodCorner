namespace FoodCorner.DTO
{
    public class CartItemDTO
    {
        public int FoodItemId { get; set; }
        public string FoodItemName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}

