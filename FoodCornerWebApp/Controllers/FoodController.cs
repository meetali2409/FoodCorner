using FoodCorner.DTO;
using FoodCornerWebApp.Helpers;
using FoodCornerWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FoodCornerWebApp.Controllers
{
    public class FoodController : Controller
    {
        private readonly HttpClient _client;

        public FoodController()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7131/")
            };
        }

        public async Task<IActionResult> Menu(int? categoryId)
        {
            var userId = HttpContext.Session.GetString("UserId");

            var foodRes = await _client.GetAsync("api/Food");
            var foodJson = await foodRes.Content.ReadAsStringAsync();
            var foodDtoList = JsonConvert.DeserializeObject<List<FoodDTO>>(foodJson) ?? new();

            var foodItems = foodDtoList.Select(f => new FoodItem
            {
                FoodItemId = f.FoodItemId,
                FoodItemName = f.FoodItemName,
                Price = f.Price,
                CategoryId = f.CategoryId,
                Image = f.Image,
                Description = f.Description
            }).ToList();


            var catRes = await _client.GetAsync("api/Category");
            var catJson = await catRes.Content.ReadAsStringAsync();
            var categories = JsonConvert.DeserializeObject<List<Category>>(catJson) ?? new();

            if (categoryId != null)
                foodItems = foodItems.Where(x => x.CategoryId == categoryId).ToList();

            List<CartItem> cartItems = new();

            if (!string.IsNullOrEmpty(userId))
            {
                var cartRes = await _client.GetAsync($"api/cart/{userId}");
                var cartJson = await cartRes.Content.ReadAsStringAsync();
                cartItems = JsonConvert.DeserializeObject<List<CartItem>>(cartJson) ?? new();
            }

          
            foreach (var food in foodItems)
            {
                var cartItem = cartItems
                    .FirstOrDefault(x => x.FoodItemId == food.FoodItemId);

                food.QuantityInCart = cartItem?.Quantity ?? 0;
            }

            var vm = new MenuViewModel
            {
                FoodItems = foodItems,
                Categories = categories,
                CartItems = cartItems
            };

            return View(vm);
        }


    }
}
