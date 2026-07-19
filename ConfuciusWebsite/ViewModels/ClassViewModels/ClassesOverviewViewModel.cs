using ConfuciusWebsite.Models;

namespace ConfuciusWebsite.ViewModels.ClassViewModels
{
    public class ClassesOverviewViewModel
    {
        public List<Classes> Classes { get; set; } = new();
        public List<ClassSignups> NewRequests { get; set; } = new();
        public List<ClassSignups> OldRequests { get; set; } = new();
    }
}
