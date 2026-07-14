using Microsoft.AspNetCore.Mvc;

namespace ConfuciusWebsite.Controllers
{
    public class ContactUsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
