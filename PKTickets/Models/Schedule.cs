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

        [Required(ErrorMessage = "Please enter the PremiumSeats")]
        [Range(10, 100, ErrorMessage = "PremiumSeats  must be between 10 to 100")]
        public int PremiumSeats { get; set; }

        [Required(ErrorMessage = "Please enter the EliteSeats")]
        [Range(10, 100, ErrorMessage = "EliteSeats  must be between 10 to 100")]
        public int EliteSeats { get; set; }

        public int AvailablePreSeats { get; set; } = 0;

        public int AvailableEliSeats { get; set; } = 0;
        public bool IsActive { get; set; } = true;

    }
}
