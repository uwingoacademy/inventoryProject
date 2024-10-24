﻿using Frontend.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Frontend.Components
{
    public class BrandCountViewComponent : ViewComponent
    {
        private readonly HttpClient _httpClient;

        public BrandCountViewComponent(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            int brandCount = 0;

            try
            {
                string apiUrl = "https://localhost:7215/api/Brand/get-brands";
                var response = await _httpClient.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var brandList = JsonSerializer.Deserialize<List<Brand>>(jsonResponse, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    brandCount = brandList?.Count ?? 0;
                }
            }
            catch (Exception ex)
            {
                brandCount = 0;  
            }

            return View(brandCount);
        }
    }

}
