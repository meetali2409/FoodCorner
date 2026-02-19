using System.ComponentModel.DataAnnotations;

public class Category
{
    [Key]
    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = null!;

    public ICollection<FoodItem> FoodItems { get; set; } = new List<FoodItem>();
}
