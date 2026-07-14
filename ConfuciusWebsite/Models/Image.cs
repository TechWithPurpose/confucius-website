namespace ConfuciusWebsite.Models
{
    public class Image
    {
        public int Id { get; set; }
        public string FilePath { get; set; } = null!;
        public string? AltText_BG { get; set; }
        public string? AltText_EN { get; set; }
        public string ItemType { get; set; } = null!;// e.g., "PageSection", "Event"
        public int ItemId { get; set; } // ID of the associated item
        public int SortOrder { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
