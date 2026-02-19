using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodCorner.DTO
{
    public class FoodItemUpdateDTO
    {
        [Key]
        public int FoodItemId { get; set; }
        [MaxLength(100)]
        public string FoodItemName { get; set; } = null!;

        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }

        public int CategoryId { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
    }
}
