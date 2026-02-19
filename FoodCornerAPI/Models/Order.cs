using System.ComponentModel.DataAnnotations.Schema;
using FoodCorner.DTO;
namespace FoodCornerAPI.Models
{
    public class Order
    {
        public int OrderId { get; set; }

        public required string CustomerName { get; set; }

        public DateTime OrderDate { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal TotalAmount { get; set; }

        public required OrderStatus Status { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}