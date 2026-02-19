using FoodCorner.DTO;
using FoodCornerWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace FoodCornerWebApp.Controllers
{
    public class CartController : Controller
    {
        private readonly HttpClient _client;

        public CartController()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7131/")
            };
        }

        public async Task<IActionResult> Add(int id, int? categoryId)
        {
            var userId = HttpContext.Session.GetString("UserId");

            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Auth");

            await _client.PostAsync(
                $"api/cart/add?foodId={id}&userId={userId}",
                null);

            return RedirectToAction("Menu", "Food");
        }


        public async Task<IActionResult> Increase(int id)
        {
            var userId = HttpContext.Session.GetString("UserId");

            await _client.PostAsync(
                $"api/cart/increase?foodId={id}&userId={userId}",
                null);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Decrease(int id)
        {
            var userId = HttpContext.Session.GetString("UserId");

            await _client.PostAsync(
                $"api/cart/decrease?foodId={id}&userId={userId}",
                null);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Remove(int id)
        {
            var userId = HttpContext.Session.GetString("UserId");

            await _client.DeleteAsync(
                $"api/cart/remove?foodId={id}&userId={userId}");

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetString("UserId");

            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Account");

            var res = await _client.GetAsync($"api/cart/{userId}");
            var json = await res.Content.ReadAsStringAsync();

            var cartItems = JsonConvert.DeserializeObject<List<CartItem>>(json);

            return View(cartItems);
        }
    }
}
