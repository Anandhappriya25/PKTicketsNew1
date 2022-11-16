using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PKTickets.Models
{
    public class Seat
    {
        [Key]
        public int SeatId { get; set; }
        [Required]
        public int ShowsId { get; set; }
        [Required]
        public int FSReserved { get; set; } = 0;
        [Required]
        public int SSReserved { get; set; } = 0;
        [Required]
        public int TSReserved { get; set; } = 0;
        public int? FOSReserved { get; set; } = 0;
        public int? BSReserved { get; set; } = 0;
        [Required]
        public int FSAvailable { get; set; } = 0;
        [Required]
        public int SSAvailable { get; set; } = 0;
        [Required]

        public int TSAvailable { get; set; } = 0;
        public int? FOSAvailable { get; set; } = 0;
        public int? BSAvailable { get; set; } = 0;
    }
}
