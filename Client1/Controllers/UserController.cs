using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Client1.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        // GET
        public IActionResult Index()
        {

            return View();
        }

        public async Task<RedirectToActionResult> LogOut()
        {
            await HttpContext.SignOutAsync("Cookies");
            await HttpContext.SignOutAsync("oidc");
        }
    }
}