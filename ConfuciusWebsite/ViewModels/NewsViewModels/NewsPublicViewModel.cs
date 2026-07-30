namespace ConfuciusWebsite.ViewModels.NewsViewModels
{
    public class NewsPublicItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime? ValidUntil { get; set; }
        public string? ImagePath { get; set; }
    }

    public class NewsPublicViewModel
    {
        public bool IsEnglish { get; set; }
        public List<NewsPublicItem> Items { get; set; } = new();
    }
}
