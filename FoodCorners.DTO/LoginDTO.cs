using System.ComponentModel.DataAnnotations;

namespace FoodCorner.DTO
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "Please enter your email")]
        [EmailAddress(ErrorMessage = "Please enter valid email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Please enter your password")]
        public string Password { get; set; }

    }
}

