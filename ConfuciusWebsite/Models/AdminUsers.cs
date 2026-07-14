using Microsoft.AspNetCore.Identity;

namespace ConfuciusWebsite.Models
{
    public class AdminUser : IdentityUser<Guid>
    {
        //public int Id { get; set; }
        // Navigation property
        public Logs Log  { get; set; }
        //public string Username { get; set; } = null!;
        //public string PasswordHash { get; set; } = null!;
        //public string UserRole { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime? LastLogin { get; set; }
    }
}
