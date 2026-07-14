namespace ConfuciusWebsite.Models
{
    public class Logs
    {
        public int Id { get; set; }
        public Guid? UserId { get; set; }
        // Navigation property
        public AdminUser? User { get; set; }
        public string Action { get; set; } = null!;
        public string EntityType { get; set; } = null!;
        public int? EntityId { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt  { get; set; }

    }
}
