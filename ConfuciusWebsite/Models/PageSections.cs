using System.ComponentModel.DataAnnotations;

namespace ConfuciusWebsite.Models
{
    public class PageSections
    {
        public int Id { get; set; }
        //Foreign key to Pages table
        public int PageId { get; set; }
        //Navigation property
        public Pages Page { get; set; }
        public string SectionType {get; set;} = null!;
        public string? Description_BG { get; set; }
        public string? Description_EN { get; set; }
        public int SortOrder {get; set;}
    }
}
