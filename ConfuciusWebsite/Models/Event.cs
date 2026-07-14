using System.ComponentModel.DataAnnotations;

namespace ConfuciusWebsite.Models
{
    public class Event
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
        public string? Photographer { get; set; }
        public string? Author { get; set; }
        public string? Translator { get; set; }
        public string Status { get; set; } = "Draft";
        public string Tickets { get; set; } = "NoTickets";
        public DateTime? DateOfEvent{ get; set; }
        public DateTime? Deadline { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
