using System;
using System.Collections.Generic;
using System.Text;

namespace FoodCorner.DTO
{
    public class UserDTO
    {
        public required string Email { get; set; } = default!;

        public required string Name { get; set; } = default!;

        public required string Role { get; set; } = default!;
    }
}
