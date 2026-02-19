using FoodCorner.DTO;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace FoodCornerWebApp.Controllers
{
    public class AdminController : Controller
    {
        private readonly HttpClient _client;

        public AdminController()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7131/")
            };
        }
        public async Task<IActionResult> MenuManage(int? editId = null, bool addMode = false)
        {
            var response = await _client.GetAsync("api/food");
            var json = await response.Content.ReadAsStringAsync();
            var foods = JsonConvert.DeserializeObject<List<FoodDTO>>(json) ?? new List<FoodDTO>();

            var catResponse = await _client.GetAsync("api/category");
            var catJson = await catResponse.Content.ReadAsStringAsync();
            var categories = JsonConvert.DeserializeObject<List<CategoryDTO>>(catJson) ?? new List<CategoryDTO>();

            ViewBag.Categories = categories;

            FoodDTO? editItem = null;
            if (editId != null)
                editItem = foods.FirstOrDefault(x => x.FoodItemId == editId);

            ViewBag.EditItem = editItem;
            ViewBag.AddMode = addMode;

            return View(foods);
        }



        [HttpPost]
        public async Task<IActionResult> Create(FoodItemCreateDTO dto)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("MenuManage");

            var content = new StringContent(JsonConvert.SerializeObject(dto),Encoding.UTF8,"application/json");
        
            await _client.PostAsync("api/food", content);

            return RedirectToAction("MenuManage");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(FoodItemUpdateDTO dto)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("MenuManage");

            var content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");
            await _client.PutAsync($"api/food/{dto.FoodItemId}", content);
            return RedirectToAction("MenuManage");
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _client.DeleteAsync($"api/food/{id}");
            return RedirectToAction("MenuManage");
        }

        public async Task<IActionResult> Index()
        {
            return await MenuManage();
        }

        public async Task<IActionResult> CategoryManage(int? editId)
        {
            var response = await _client.GetAsync("api/category");
            var json = await response.Content.ReadAsStringAsync();
            var categories = JsonConvert.DeserializeObject<List<Category>>(json);

            Category? editItem = null;

            if (editId != null)
                editItem = categories.FirstOrDefault(x => x.CategoryId == editId);

            ViewBag.EditItem = editItem;

            return View(categories);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory(Category dto)
        {
            var content = new StringContent(JsonConvert.SerializeObject(dto),Encoding.UTF8,"application/json");

            await _client.PostAsync("api/category", content);
            return RedirectToAction("CategoryManage");
        }

        [HttpPost]
        public async Task<IActionResult> EditCategory(Category dto)
        {
            var content = new StringContent( JsonConvert.SerializeObject(dto),Encoding.UTF8,"application/json" );

            await _client.PutAsync($"api/category/{dto.CategoryId}", content);
            return RedirectToAction("CategoryManage");
        }

        public async Task<IActionResult> DeleteCategory(int id)
        {
            await _client.DeleteAsync($"api/category/{id}");
            return RedirectToAction("CategoryManage");
        }
        public async Task<IActionResult> OrderManage(int? detailsId = null)
        {
            var response = await _client.GetAsync("api/orders");
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ApiResponse<List<OrderResponseDTO>>>(json);

            OrderResponseDTO? selectedOrder = null;

            if (detailsId != null)
            {
                var orderResponse = await _client.GetAsync($"api/orders/{detailsId}");
                var orderJson = await orderResponse.Content.ReadAsStringAsync();
                var orderResult = JsonConvert.DeserializeObject<ApiResponse<OrderResponseDTO>>(orderJson);

                selectedOrder = orderResult?.Data;
            }

            ViewBag.SelectedOrder = selectedOrder;

            return View(result?.Data ?? new List<OrderResponseDTO>());
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int id, int status)
        {
            var body = new { Status = status };
            var content = new StringContent(JsonConvert.SerializeObject(body),Encoding.UTF8,"application/json");

            await _client.PutAsync($"api/orders/{id}/status", content);

            return RedirectToAction("OrderManage");

        }

    }
}
