using Microsoft.AspNetCore.Mvc;

namespace YunusExpress_MVC.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
