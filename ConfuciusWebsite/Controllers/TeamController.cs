using Microsoft.AspNetCore.Mvc;

namespace ConfuciusWebsite.Controllers
{
    public class TeamController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
