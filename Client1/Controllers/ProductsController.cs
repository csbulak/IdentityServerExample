using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Client1.Models;
using Client1.Services;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Newtonsoft.Json;

namespace Client1.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IApiResource _apiResource;

        public ProductsController(IConfiguration configuration, IApiResource apiResource)
        {
            _configuration = configuration;
            _apiResource = apiResource;
        }

        public async Task<IActionResult> Index()
        {
            var response = await _apiResource.GetHttpClient().Result.GetAsync("https://localhost:5016/api/Products/GetProducts");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                var products = JsonConvert.DeserializeObject<List<Product>>(content);
                return View(products);
            }
            else
            {
                //loglama yap
            }

            return View();
        }
    }
}
