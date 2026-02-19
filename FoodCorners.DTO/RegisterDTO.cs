
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FoodCorner.DTO
{
    public class RegisterDTO
    {
        [Required(ErrorMessage = "Please enter your name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please enter your email")]
        [EmailAddress(ErrorMessage = "Please enter valid email")]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required(ErrorMessage = "Please enter your role")]
        public string Role { get; set; }
    }
}