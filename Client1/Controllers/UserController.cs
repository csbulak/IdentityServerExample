using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace Client1.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly IConfiguration _configuration;

        public UserController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // GET
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync("Cookies");

            return RedirectToAction("Index", "Home");
            //await HttpContext.SignOutAsync("oidc");
        }

        public async Task<IActionResult> GetRefreshToken()
        {
            HttpClient httpClient = new HttpClient();

            var refreshToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);

            var discovery = await httpClient.GetDiscoveryDocumentAsync("https://localhost:5001");
            if (discovery.IsError)
            {
                //loglama yap
            }

            var token = await httpClient.RequestRefreshTokenAsync(new RefreshTokenRequest
            {
                ClientId = _configuration["Client2:ClientId"],
                ClientSecret = _configuration["Client2:ClientSecret"],
                RefreshToken = refreshToken,
                Address = discovery.TokenEndpoint,

            });

            if (token.IsError)
            {
                // Yönlendirme yap
            }

            var tokens = new List<AuthenticationToken>()
            {
                new AuthenticationToken()
                {
                    Name = OpenIdConnectParameterNames.IdToken,
                    Value = token.IdentityToken
                },
                new AuthenticationToken()
                {
                    Name = OpenIdConnectParameterNames.AccessToken,
                    Value = token.AccessToken
                },
                new AuthenticationToken()
                {
                    Name = OpenIdConnectParameterNames.RefreshToken,
                    Value = token.RefreshToken
                },
                new AuthenticationToken()
                {
                    Name = OpenIdConnectParameterNames.ExpiresIn,
                    Value = DateTime.UtcNow.AddSeconds(token.ExpiresIn).ToString("o", CultureInfo.InvariantCulture)
                }
            };

            var authenticationResult = await HttpContext.AuthenticateAsync();

            var properties = authenticationResult.Properties;

            properties?.StoreTokens(tokens);

            if (authenticationResult.Principal != null)
                await HttpContext.SignInAsync("Cookies", authenticationResult.Principal, properties);

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "admin")]
        public IActionResult AdminAction()
        {
            return View();
        }

        [Authorize(Roles = "customer, admin")]
        public IActionResult CustomerAction()
        {
            return View();
        }
    }
}