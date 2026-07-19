namespace ConfuciusWebsite.ViewModels.ClassViewModels
{
    public class ScheduleItem
    {
        public int Id { get; set; }
        public string DayOfWeek { get; set; } = "Monday";
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
    }
}
