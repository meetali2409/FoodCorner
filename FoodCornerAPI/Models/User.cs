using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace FoodCornerAPI.Models
{
    public class User : IdentityUser
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}

