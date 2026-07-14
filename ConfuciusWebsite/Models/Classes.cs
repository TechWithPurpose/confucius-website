using System.ComponentModel.DataAnnotations;

namespace ConfuciusWebsite.Models
{
    public class Classes
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
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation properties
        public List<ClassSchedule> Schedules { get; set; } 
        public List<ClassSignups> Signups { get; set; }
    }
}
