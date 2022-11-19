using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PKTickets.Models
{
    public class Reservation
    {
        [Key]
        public int ReservationId { get; set; }

       

        [Required(ErrorMessage = "Please Enter the User Id !!!")]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User? User { get; set; }

       
        [Required(ErrorMessage = "Please Enter the Schedule Id !!!")]
        public int ScheduleId { get; set; }

        [ForeignKey("ScheduleId")]
        public Schedule? Schedule { get; set; }


        [Required(ErrorMessage = "Please enter the NumberOfTickets")]
        [Range(1, 10, ErrorMessage = "NumberOfTickets seats must be between 1 to 10")]
        public int NumberOfTickets { get; set; } 
        public bool IsActive { get; set; }=true;

    }
}