using System.ComponentModel.DataAnnotations;
using ConfuciusWebsite.ViewModels.EventViewModels;

namespace ConfuciusWebsite.ViewModels.ClassViewModels
{
    public class ClassEdit
    {
        public int Id { get; set; }
        [Required]
        public string Title_BG { get; set; } = null!;
        [Required]
        public string Title_EN { get; set; } = null!;
        [Required]
        public string Description_BG { get; set; } = null!;
        [Required]
        public string Description_EN { get; set; } = null!;
        public DateTime? StartDate { get; set; }
        public List<ScheduleItem> Schedules { get; set; } = new();
        public List<ImageOption>? AvailableImages { get; set; }
    }
}
