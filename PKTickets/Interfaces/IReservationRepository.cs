using PKTickets.Models;
using PKTickets.Models.DTO;

namespace PKTickets.Interfaces
{
    public interface IReservationRepository
    {

        public List<Reservation> ReservationList();
        public Reservation ReservationById(int id);
        public Messages DeleteReservation(int id);
        public Messages CreateReservation(ReservationDTO reservation);
        public Messages UpdateReservation(ReservationDTO reservation);
        public List<Reservation> ReservationsByShowId(int id);
    }
}
