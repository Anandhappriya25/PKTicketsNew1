namespace PKTickets.Models.DTO
{
    public class MovieDTO
    {
        public string MovieName { get; set; }
        public List<TheaterDetails> Theaters { get; set; }
    }
    public class TheaterDetails
    {
        public string TheaterId { get; set; }
        public string TheaterName { get; set; }
        public List<ScreenDetails> Screens { get; set; }
    }
    public class ScreenDetails
    {
        public string ScreenId { get; set; }
        public string ScreenName { get; set; }
        public int PremiumCapacity { get; set; }
        public int EliteCapacity { get; set; }
        public List<ScheduleDetails> Schedules { get; set; }
    }
    public class ScheduleDetails
    {
        public int ScheduleId { get; set; }
        public DateTime Date { get; set; }
        public string ShowTime { get; set; }
        public int AvailablePremiumSeats { get; set; }
        public int AvailableEliteSeats { get; set; }
       
    }
}
