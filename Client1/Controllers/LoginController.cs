using Microsoft.AspNetCore.Mvc;

namespace Client1.Controllers
{
    public class LoginController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }
    }
}