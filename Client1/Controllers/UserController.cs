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
    }
}