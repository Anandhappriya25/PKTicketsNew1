using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PKTickets.Models
{
    public class Show
    {
        [Key]
        public int ShowId { get; set; }
       
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
        public bool IsActive { get; set; } = true;

    }
}
