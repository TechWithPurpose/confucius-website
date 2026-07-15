using Microsoft.AspNetCore.Identity;

namespace ConfuciusWebsite.Models
{
    public class AdminUser : IdentityUser<Guid>
    {
        // Navigation property
        public Logs Log  { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastLogin { get; set; }
    }
}
