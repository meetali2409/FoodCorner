using FoodCornerWebApp.Models;

public class MenuViewModel
    {
    public List<Category> Categories { get; set; } = new();
    public List<FoodItem> FoodItems { get; set; } = new();
    public List<CartItem> CartItems { get; set; } = new();
}
