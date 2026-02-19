using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodCornerWebApp.Models
{
        public class FoodItem
        {
            public int FoodItemId { get; set; }
            public string FoodItemName { get; set; }
            public decimal Price { get; set; }
            public int CategoryId { get; set; }

            public string CategoryName { get; set; }

            public string Image { get; set; }
            public string Description { get; set; }

            public int QuantityInCart { get; set; }
        }
    }