namespace FoodCornerWebApp.Models
{
    public class CartItem
    {
        public int FoodItemId { get; set; }
        public string FoodItemName { get; set; }
        public string CategoryName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string Image { get; set; }

        public decimal Total => Price * Quantity;
    }


}
