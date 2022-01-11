using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Client1.Models;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace Client1.Controllers
{
    public class LoginController : Controller
    {
        private IConfiguration _configuration;

        public LoginController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // GET
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginViewModel loginViewModel)
        {
            var client = new HttpClient();

            var disco = await client.GetDiscoveryDocumentAsync(_configuration["AuthServerUrl"]);

            if (disco.IsError)
            {
                ModelState.AddModelError("", "Email veya şifreniz yanlış.");
                return View();
                // Hata ve Loglama 
            }

            var token = await client.RequestPasswordTokenAsync(new PasswordTokenRequest()
            {
                ClientId = _configuration["Client1-ResourceOwner:ClientId"],
                ClientSecret = _configuration["Client1-ResourceOwner:ClientSecret"],
                Address = disco.TokenEndpoint,
                UserName = loginViewModel.Email,
                Password = loginViewModel.Password
            });

            if (token.IsError)
            {
                ModelState.AddModelError("", "Email veya şifreniz yanlış.");
                return View();
            }

            var userInfo = await client.GetUserInfoAsync(new UserInfoRequest()
            {
                Token = token.AccessToken,
                Address = disco.UserInfoEndpoint
            });

            if (userInfo.IsError)
            {

            }

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(userInfo.Claims, CookieAuthenticationDefaults.AuthenticationScheme, "name", "role");

            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            var authenticationProperties = new AuthenticationProperties();

            authenticationProperties.StoreTokens(new List<AuthenticationToken>()
            {
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
            });


            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, authenticationProperties);

            return RedirectToAction("Index", "User");
        }
    }
}