namespace FoodCorner.DTO
{
    public class OrderItemUpdateDTO
    {
        public int OrderItemId { get; set; }

        public int OrderId { get; set; }

        public int FoodItemId { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }
    }
}

