﻿using Frontend.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json;

namespace Frontend.Controllers
{
    public class SupplierController : Controller
    {
        private readonly HttpClient _httpClient;
        public SupplierController(HttpClient httpClient)
        {

            _httpClient = httpClient;

        }
        public async Task<IActionResult> Index()
        {
            try
            {
                string apiUrl = "https://localhost:7215/api/Supplier/get-suppliers";
                var response = await _httpClient.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var model = System.Text.Json.JsonSerializer.Deserialize<List<Supplier>>(jsonResponse, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    return View(model);
                }
                else
                {
                    ViewBag.ErrorMessage = "There was an error fetching data from the API.";
                    return View(new List<Supplier>());
                }

            }
            catch (Exception ex)
            {

                return View("Error");
            }


        }
        [HttpPost]
        public async Task Create([FromBody] Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                var jsonData = JsonConvert.SerializeObject(supplier);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                await _httpClient.PostAsync("https://localhost:7215/api/Supplier/create-supplier", content);

            }


        }
        [HttpPost]
        public async Task Update([FromBody] Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                var jsonData = JsonConvert.SerializeObject(supplier);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                await _httpClient.PostAsync("https://localhost:7215/api/Supplier/update-supplier", content);

            }


        }
        [HttpPost]
        public async Task Delete([FromBody] int id)
        {
            if (ModelState.IsValid)
            {
                await _httpClient.DeleteAsync("https://localhost:7215/api/Supplier/delete-supplier/" + id);
            }
        }
    }
}
