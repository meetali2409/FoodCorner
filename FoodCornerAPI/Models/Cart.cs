namespace FoodCornerAPI.Models
{
    public class Cart
    {
        public int CartId { get; set; }
        public string UserId { get; set; }
        public int FoodItemId { get; set; }
        public int Quantity { get; set; }

        public FoodItem FoodItem { get; set; }
    }

}
