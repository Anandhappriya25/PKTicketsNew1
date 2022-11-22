using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PKTickets.Models.DTO
{
    public class ReservationDTO
    {
        public int ReservationId { get; set; }

        public int UserId { get; set; }
        public int ScheduleId { get; set; }
        public int NumberOfTickets { get; set; }
        public int PremiumTickets { get; set; }
        public int EliteTickets { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
