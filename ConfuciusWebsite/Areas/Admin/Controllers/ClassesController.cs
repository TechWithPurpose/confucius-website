using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConfuciusWebsite.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]

    public class ClassesController : Controller
    {

        [HttpGet]
        public IActionResult CreateClass()
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
