using FoodCorner.DTO;
using FoodCornerWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace FoodCornerWebApp.Controllers
{
    public class OrderController : Controller
    {
        private readonly HttpClient _client;

        public OrderController()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7131/")
            };
        }

        public async Task<IActionResult> Checkout()
        {
            var userId = HttpContext.Session.GetString("UserId");

            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Auth");

            var response = await _client.GetAsync($"api/cart/{userId}");
            var json = await response.Content.ReadAsStringAsync();

            var cartItems = JsonConvert.DeserializeObject<List<CartItem>>(json);

            if (cartItems == null || !cartItems.Any())
                return RedirectToAction("Index", "Home");

            return View(cartItems);
        }

    
        [HttpPost]
        public async Task<IActionResult> PlaceOrder()
        {
            var userId = HttpContext.Session.GetString("UserId");
            var customerName = HttpContext.Session.GetString("UserName");

            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Auth");

            var cartResponse = await _client.GetAsync($"api/cart/{userId}");
            var cartJson = await cartResponse.Content.ReadAsStringAsync();
            var cartItems = JsonConvert.DeserializeObject<List<CartItem>>(cartJson);

            if (cartItems == null || !cartItems.Any())
                return RedirectToAction("Index", "Home");

            var order = new
            {
                UserId = userId,   
                CustomerName = customerName,
                Items = cartItems.Select(x => new
                {
                    FoodItemId = x.FoodItemId,
                    Quantity = x.Quantity
                }).ToList()
            };

            var content = new StringContent(
                JsonConvert.SerializeObject(order),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _client.PostAsync("api/orders", content);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                return Content("API Error: " + error);
            }

      
            await _client.DeleteAsync($"api/cart/clear/{userId}");

            return RedirectToAction("OrderSuccess");
        }

        public IActionResult OrderSuccess()
        {
            return View();
        }
    }
}
