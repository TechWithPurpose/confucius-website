using ConfuciusWebsite.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;

namespace ConfuciusWebsite.Data
{
    public class ApplicationDbContext : IdentityDbContext<AdminUser, IdentityRole<Guid>, Guid>
    {
         
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
           
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Rename tables
            builder.Entity<AdminUser>().ToTable("Users");
            builder.Entity<IdentityRole<Guid>>().ToTable("Roles");
            builder.Entity<IdentityUserRole<Guid>>().ToTable("UserRoles");
            builder.Entity<IdentityUserClaim<Guid>>().ToTable("UserClaims");
            builder.Entity<IdentityUserLogin<Guid>>().ToTable("UserLogins");
            builder.Entity<IdentityRoleClaim<Guid>>().ToTable("RoleClaims");
            builder.Entity<IdentityUserToken<Guid>>().ToTable("UserTokens");
            // Seed initial roles using HasData
            var adminRoleId = Guid.Parse("c8d89a25-4b96-4f20-9d79-7f8a54c5213d");
            var guestRoleId = Guid.Parse("f2e6b8a1-9d43-4a7c-9f32-71d7c5dbe9f0");
            builder.Entity<IdentityRole<Guid>>().HasData(
                new IdentityRole<Guid> { Id = adminRoleId, Name = "Admin", NormalizedName = "ADMIN"},
                new IdentityRole<Guid> { Id = guestRoleId, Name = "Guest", NormalizedName = "GUEST"}
            );

            // Configure one-to-one relationship between AdminUser and Logs
            builder.Entity<AdminUser>()
            .HasOne(u => u.Log)
            .WithOne(l => l.User)
            .HasForeignKey<Logs>(l => l.UserId);
        }
        // This is what tells EF Core:
        //“These tables exist.Please track them.”
        public DbSet<AdminUser> AdminUsers { get; set; }
        public DbSet<AskAQuestionEmails> AskAQuestionEmails { get; set; }
        public DbSet<Classes> Classes { get; set; }
        public DbSet<ClassSchedule> ClassSchedule { get; set; }
        public DbSet<ClassSignups> ClassSignups { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Models.Image> Images { get; set; }
        public DbSet<Logs> Logs { get; set; }
        public DbSet<NavigationList> NavigationList { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<Pages> Pages { get; set; }
        public DbSet<PageSections> PageSections { get; set; }
        public DbSet<Settings> Settings { get; set; }
    }
}
