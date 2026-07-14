namespace ConfuciusWebsite.Models
{
    public class ClassSchedule
    {
        public int Id { get; set; }
        // Foreign key to Classes table
        public int ClassId { get; set; }
        // Navigation property
        public Classes Class { get; set; } = null!;
        public string DayOfWeek { get; set; } = null!;
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }

    }
}
