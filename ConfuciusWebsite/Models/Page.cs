using System.ComponentModel.DataAnnotations;

namespace ConfuciusWebsite.Models
{
    public class Pages
    {
        public int Id { get; set; }
        [Required]
        public string MenuLabel_BG { get; set; } = null!;
        [Required]
        public string MenuLabel_EN { get; set; } = null!;
        [Required]
        public Boolean IsVisible { get; set; }
        [Required]
        public string Title_BG { get; set; } = null!;
        [Required]
        public string Title_EN { get; set; } = null!;
        public string Slugs { get; set; } = null!;
        public string Status { get; set; } = "Draft";
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public int SortOrder { get; set; }

        // Navigation property (one page → many sections)
        public List<PageSections> Sections { get; set; }
        public List<NavigationList> NavigationItems { get; set; }
    }
}
