using FoodCorner.DTO;
using FoodCornerWebApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;

namespace FoodCornerWebApp.Controllers
{
    public class AuthController : Controller
    {
        private readonly HttpClient _client;

        public AuthController()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7131/")
            };
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterDTO dto)
        {
            var content = new StringContent(JsonConvert.SerializeObject(dto),Encoding.UTF8,"application/json" );

            var response = await _client.PostAsync("api/user/register", content);

            if (!response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var passwordErrors = new List<string>();

                if (json.TrimStart().StartsWith("["))
                {
                    var errors = JsonConvert.DeserializeObject<List<FoodCornerWebApp.Models.ApiError>>(json);

                    passwordErrors = errors
                        .Where(e => e.Code.StartsWith("Password"))
                        .Select(e => e.Description)
                        .ToList();
                }
                else
                {
                    var obj = JsonConvert.DeserializeObject<Newtonsoft.Json.Linq.JObject>(json);

                    if (obj["errors"]?["Password"] != null)
                    {
                        foreach (var err in obj["errors"]["Password"])
                        {
                            passwordErrors.Add(err.ToString());
                        }
                    }
                }

                ViewBag.PasswordErrors = passwordErrors;
                return View(dto);
            }

            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO dto)
        {
            var content = new StringContent(JsonConvert.SerializeObject(dto),Encoding.UTF8,"application/json");

            var response = await _client.PostAsync("api/user/login", content);

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "Invalid username or password";
                return View(dto);
            }

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<LoginResponse>(json);

            if (result == null)
            {
                ViewBag.Error = "Login failed";
                return View(dto);
            }

            HttpContext.Session.SetString("UserId", result.UserId);
            HttpContext.Session.SetString("UserName", result.Name);
            HttpContext.Session.SetString("UserRole", result.Role ?? "");

            if (result.Role == "Admin")
                return RedirectToAction("Index", "Admin");

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
            return RedirectToAction("Login");
        }
    }
}
