using Microsoft.AspNetCore.Mvc;

namespace ConfuciusWebsite.Controllers
{
    public class EventsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        
    }
}
