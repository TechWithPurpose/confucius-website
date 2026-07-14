using Microsoft.AspNetCore.Mvc;

namespace ConfuciusWebsite.Controllers
{
    public class NewsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
