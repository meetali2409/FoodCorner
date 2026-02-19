public class FoodItemCreateDTO
{
    public string FoodItemName { get; set; }
    public decimal Price { get; set; }
    public int CategoryId { get; set; }  
    public string? Description { get; set; }
    public string? Image { get; set; }
}
