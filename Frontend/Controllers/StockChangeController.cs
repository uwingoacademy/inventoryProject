using Frontend.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json;

namespace Frontend.Controllers
{
    public class StockChangeController : Controller
    {
        private readonly HttpClient _httpClient;

        public StockChangeController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                string apiUrl = "https://localhost:7215/api/StockChange/get-stock-changes";
                var response = await _httpClient.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                  var model = System.Text.Json.JsonSerializer.Deserialize<List<StockChangeViewModel>>(jsonResponse, new JsonSerializerOptions
                  {
                        PropertyNameCaseInsensitive = true
                    });
                    return View(model);
                }
                else
                {
                    ViewBag.ErrorMessage = "There was an error fetching data from the API.";
                    return View(new List<StockChange>());
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred: " + ex.Message;
                return View(new List<StockChange>());
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromBody] StockChange stockChange)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var jsonData = JsonConvert.SerializeObject(stockChange);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("https://localhost:7215/api/StockChange/create-stock-change", content);

            if (response.IsSuccessStatusCode)
            {
                return Ok();
            }

            return BadRequest("Error creating stock change.");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromBody] StockChange stockChange)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var jsonData = JsonConvert.SerializeObject(stockChange);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("https://localhost:5001/api/StockChange/update-stock-change", content);

            if (response.IsSuccessStatusCode)
            {
                return Ok();
            }

            return BadRequest("Error updating stock change.");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpClient.DeleteAsync($"https://localhost:5001/api/StockChange/delete-stock-change?id={id}");
            if (response.IsSuccessStatusCode)
            {
                return Ok();
            }

            return BadRequest("Error deleting stock change.");
        }
    }
}
