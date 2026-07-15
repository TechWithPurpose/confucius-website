using System.ComponentModel.DataAnnotations;
using ConfuciusWebsite.ViewModels.EventViewModels;

namespace ConfuciusWebsite.ViewModels.NewsViewModels
{
    public class NewsEdit
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
        public DateTime? DateOfEvent { get; set; }
        public string Status { get; set; } = "Draft";
        public string? Tickets { get; set; }
        public DateTime? ValidUntil { get; set; }
        public List<ImageOption>? AvailableImages { get; set; }
    }
}
