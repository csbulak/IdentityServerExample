using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Client1.Models;
using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Client1.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IConfiguration _configuration;

        public ProductsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {
            HttpClient client = new HttpClient();

            var discovery = await client.GetDiscoveryDocumentAsync("https://localhost:5001");
            if (discovery.IsError)
            {
                //loglama yap
            }

            var token = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest()
            {
                ClientId = _configuration["Client:ClientId"],
                ClientSecret = _configuration["Client:ClientSecret"],
                Address = discovery.TokenEndpoint
            });

            if (token.IsError)
            {
                //loglama yap
            }

            //https://localhost:5016

            client.SetBearerToken(token.AccessToken);

            var response = await client.GetAsync("https://localhost:5016/api/Products/GetProducts");

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
