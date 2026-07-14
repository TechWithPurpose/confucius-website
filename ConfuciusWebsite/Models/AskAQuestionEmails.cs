namespace ConfuciusWebsite.Models
{
    public class AskAQuestionEmails
    {
        public int Id { get; set; }
        public string Email { get; set; } = null!;
        public Boolean isActive { get; set; }
    }
}
