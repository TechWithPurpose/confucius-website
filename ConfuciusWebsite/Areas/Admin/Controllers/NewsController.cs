using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConfuciusWebsite.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]

    public class NewsController : Controller
    {

        [HttpGet]
        public IActionResult CreateNews()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Overview()
        {

            return View();
        }
    }
}
