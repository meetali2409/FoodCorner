using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodCorner.DTO
{
    public class FoodDTO
    {
        public int FoodItemId { get; set; }
        public string FoodItemName { get; set; } = null!;
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;
   
        public string? Image { get; set; }
        public string? Description { get; set; }
    }


}
