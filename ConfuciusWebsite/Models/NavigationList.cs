namespace ConfuciusWebsite.Models
{
    public class NavigationList
    {
        public int Id { get; set; }
        //Foreign key to Pages table
        public int? PageId { get; set; }

        //Navigation property
        public Pages Page { get; set; }
        public string Type  { get; set; } = null!;
        public string Label_BG { get; set; } = null!;
        public string Label_EN { get; set; } = null!;
        public int Position { get; set; }
        public Boolean IsVisible { get; set; }
    }
}
