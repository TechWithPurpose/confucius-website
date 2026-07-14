using Microsoft.AspNetCore.Mvc;

namespace ConfuciusWebsite.Controllers
{
    public class ClassesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
