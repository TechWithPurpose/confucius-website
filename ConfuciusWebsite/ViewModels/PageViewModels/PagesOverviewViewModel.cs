using ConfuciusWebsite.Models;

namespace ConfuciusWebsite.ViewModels.PageViewModels
{
    public class PagesOverviewViewModel
    {
        public List<Pages> Pages { get; set; } = new();
        public List<NavigationList> NavigationItems { get; set; } = new();
    }
}
