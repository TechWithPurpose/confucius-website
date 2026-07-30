using ConfuciusWebsite.Data;
using ConfuciusWebsite.ViewModels.NewsViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ConfuciusWebsite.Controllers
{
    public class NewsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NewsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // Same "Lang" session key the NavMenu view component reads - defaults to
            // Bulgarian until a real language switcher is built.
            var lang = HttpContext.Session.GetString("Lang") ?? "BG";
            var isEnglish = lang == "EN";

            var today = DateTime.Now;

            // Only show Published items that haven't passed their expiration date.
            // Sorted by the news item's own date (DateOfEvent), falling back to
            // CreatedAt for older rows that don't have one set.
            var newsList = _context.News
                .Where(n => n.Status == "Published" && (n.ValidUntil == null || n.ValidUntil >= today))
                .OrderByDescending(n => n.DateOfEvent ?? n.CreatedAt)
                .ToList();

            var newsIds = newsList.Select(n => n.Id).ToList();

            // Grab every image for these items in one query, then pick the first
            // (lowest SortOrder) per item, instead of querying per news row.
            var allImages = _context.Images
                .Where(i => i.ItemType == "News" && newsIds.Contains(i.ItemId))
                .OrderBy(i => i.SortOrder)
                .ToList();

            var firstImageByNewsId = allImages
                .GroupBy(i => i.ItemId)
                .ToDictionary(g => g.Key, g => g.First().FilePath);

            var vm = new NewsPublicViewModel
            {
                IsEnglish = isEnglish,
                Items = newsList.Select(n => new NewsPublicItem
                {
                    Id = n.Id,
                    Title = isEnglish ? n.Title_EN : n.Title_BG,
                    Description = isEnglish ? n.Description_EN : n.Description_BG,
                    ValidUntil = n.ValidUntil,
                    ImagePath = firstImageByNewsId.TryGetValue(n.Id, out var path) ? path : null
                }).ToList()
            };

            return View(vm);
        }
    }
}
