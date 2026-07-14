using System.ComponentModel.DataAnnotations;

namespace ConfuciusWebsite.Models
{
    public class ClassSignups
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = null!;
        [Required]
        public int Age { get; set; }
        [Required]
        public string Email { get; set; } = null!;
        public string? Phone { get; set; }
        // Foreign key to Classes table
        public int ClassId { get; set; }
        // Navigation property
        public Classes Class { get; set; }
        [Required]
        public string ClassType { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public string? HSKLevel { get; set; }
    }
}
