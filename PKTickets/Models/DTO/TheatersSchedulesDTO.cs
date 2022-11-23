namespace PKTickets.Models.DTO
{
    public class TheatersSchedulesDTO
    {
        public string TheaterName { get; set; }
        public int ScreensCount { get; set; }
        public List<ScreenSchedulesDTO> Screens { get; set; }
  

    }

    //public class ScreenSchedules
    //{
    //    public int ScheduleId { get; set; }
    //    public string MovieName { get; set; }
    //    public DateTime Date { get; set; }
    //    public string ShowTime { get; set; }

    //    public int AvailablePremiumSeats { get; set; }
    //    public int AvailableEliteSeats { get; set; }
    //}
    public class ScreenSchedulesDTO
    {
        public int ScreenId { get; set; }
        public string ScreenName { get; set; }
        public int PremiumCapacity { get; set; }
        public int EliteCapacity { get; set; }
        public List<SchedulesDTO> Schedules { get; set; }
    }

 }
