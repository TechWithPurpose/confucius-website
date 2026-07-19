using ConfuciusWebsite.Data;
using ConfuciusWebsite.Models;
using ConfuciusWebsite.ViewModels.PageViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConfuciusWebsite.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class PagesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PagesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Overview()
        {
            var model = new PagesOverviewViewModel
            {
                Pages = _context.Pages
                    .OrderBy(p => p.Title_EN)
                    .ToList(),

                NavigationItems = _context.NavigationList
                    .OrderBy(n => n.Position)
                    .ToList()
            };

            return View(model);
        }

        // Placeholder for now - the actual page editor/builder is a separate piece of work.
        [HttpGet]
        public IActionResult CreatePage()
        {
            return View();
        }

        // Placeholder for now - same as above.
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var page = _context.Pages.FirstOrDefault(p => p.Id == id);
            if (page == null)
            {
                return NotFound();
            }

            return View(page);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var pageToDelete = _context.Pages.FirstOrDefault(p => p.Id == id);
            if (pageToDelete == null)
            {
                return NotFound();
            }

            // NavigationList.PageId has no cascade delete configured, so clear any references
            // first or deleting the page would throw a foreign key constraint error.
            var linkedNavItems = _context.NavigationList.Where(n => n.PageId == id).ToList();
            foreach (var navItem in linkedNavItems)
            {
                navItem.PageId = null;
            }

            // PageSections cascade-delete automatically at the DB level.
            _context.Pages.Remove(pageToDelete);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Page deleted successfully.";
            return RedirectToAction("Overview", "Pages", new { area = "Admin" });
        }

        // Called via fetch() from Overview.cshtml after a drag-and-drop reorder.
        // Body is a JSON array of NavigationList ids in their new order, e.g. [3, 1, 2, 4]
        [HttpPost]
        public IActionResult SaveNavigationOrder([FromBody] List<int> orderedIds)
        {
            if (orderedIds == null || orderedIds.Count == 0)
            {
                return BadRequest();
            }

            var items = _context.NavigationList
                .Where(n => orderedIds.Contains(n.Id))
                .ToList();

            for (int i = 0; i < orderedIds.Count; i++)
            {
                var item = items.FirstOrDefault(n => n.Id == orderedIds[i]);
                if (item != null)
                {
                    item.Position = i;
                }
            }

            _context.SaveChanges();

            return Ok();
        }
    }
}
