using ConfuciusWebsite.Data;
using Microsoft.AspNetCore.Mvc;

namespace ConfuciusWebsite.ViewComponents
{
    public class NavMenuItem
    {
        public string Label { get; set; } = null!;
        public string Url { get; set; } = null!;
    }

    public class NavMenuViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public NavMenuViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            // "Lang" isn't wired up to a real language switcher yet - default to Bulgarian
            // until that feature exists. Once the switcher is built, it should just set
            // HttpContext.Session.SetString("Lang", "EN") or "BG" and this will follow it.
            var lang = HttpContext.Session.GetString("Lang") ?? "BG";
            var isEnglish = lang == "EN";

            var items = _context.NavigationList
                .Where(n => n.IsVisible)
                .OrderBy(n => n.Position)
                .ToList();

            var menuItems = new List<NavMenuItem>();

            foreach (var item in items)
            {
                string? url = item.Type switch
                {
                    "News" => Url.Action("Index", "News"),
                    "Events" => Url.Action("Index", "Events"),
                    "Classes" => Url.Action("Index", "Classes"),
                    "ContactUs" => Url.Action("Index", "ContactUs"),
                    "Team" => Url.Action("Index", "Team"),
                    // "Page" items would link to a CMS-driven page by slug once that
                    // feature is built (the "+ Add Page" work still pending).
                    _ => null
                };

                if (url == null)
                {
                    continue;
                }

                menuItems.Add(new NavMenuItem
                {
                    Label = isEnglish ? item.Label_EN : item.Label_BG,
                    Url = url
                });
            }

            return View(menuItems);
        }
    }
}
