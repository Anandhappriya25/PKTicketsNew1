using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PKTickets.Models
{
    public class Schedule
    {
        [Key]
        public int ScheduleId { get; set; }
       
        [Required(ErrorMessage = "Please Enter the Screen Id !!!")]
        public int ScreenId { get; set; }

        [ForeignKey("ScreenId")]
        public Screen? Screen { get; set; }
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Please Enter the ShowTime Id !!!")]
        public int ShowTimeId { get; set; }

        [ForeignKey("ShowTimeId")]
        public ShowTime? ShowTime { get; set; }

        [Required(ErrorMessage = "Please Enter the Movie Id !!!")]
        public int MovieId { get; set; }

        [ForeignKey("MovieId")]
        public Movie? Movie { get; set; }

    
        public int PremiumSeats { get; set; }

        public int EliteSeats { get; set; }

        public int AvailablePreSeats { get; set; } 

        public int AvailableEliSeats { get; set; } 
        public bool IsActive { get; set; } = true;

    }
}
