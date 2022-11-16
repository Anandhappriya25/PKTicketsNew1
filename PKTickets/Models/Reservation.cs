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

        [Required(ErrorMessage = "Please Enter the Seat Id !!!")]
        public int SeatId { get; set; }

        [ForeignKey("SeatId")]
        public Seat? Seat { get; set; }
        [Required(ErrorMessage = "Please Enter the Show Id !!!")]
        public int ShowId { get; set; }

        [ForeignKey("ShowId")]
        public Show? Show { get; set; }

        [Required(ErrorMessage = "Please Enter the PayType Id !!!")]
        public int PayTypeId { get; set; }

        [ForeignKey("PayTypeId")]
        public PayType? PayType { get; set; }


        [Required(ErrorMessage = "Please enter the NumberOfTickets")]
        [Range(1, 10, ErrorMessage = "NumberOfTickets seats must be between 1 to 10")]
        public int NumberOfTickets { get; set; } = 0;
        public int FSBooked { get; set; } = 0;
        public int SSBooked { get; set; } = 0;
        public int TSBooked { get; set; } = 0;
        public int? FOSBooked { get; set; } = 0;
        public int? BSBooked { get; set; } = 0;
        public bool IsActive { get; set; }=true;

    }
}
//user id
//reservation id
//paytypeid
//fsbooked
//no of ticekts
//show id
//is active