using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PKTickets.Interfaces;
using PKTickets.Models;
using PKTickets.Models.DTO;

namespace PKTickets.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {

        private readonly IReservationRepository reservationRepository;

        public ReservationsController(IReservationRepository _reservationRepository)
        {
            reservationRepository = _reservationRepository;
        }


        [HttpGet("ReservationList")]
        public IActionResult ReservationList()
        {
            var reservation = reservationRepository.ReservationList();
            if (reservation.Count() == 0)
            {
                throw new Exception("The Reservation list is empty");
            }
            return Ok(reservation);
        }

        [HttpGet("ReservationsByShowId")]
        public IActionResult ReservationsByShowId(int id)
        {
            var reservation = reservationRepository.ReservationsByShowId(id);
            if (reservation.Count() == 0)
            {
                throw new Exception("The Reservation list is empty for this Show");
            }
            return Ok(reservation);
        }

        [HttpGet("ReservationById/{id}")]

        public ActionResult ReservationById(int id)
        {
            var reservation = reservationRepository.ReservationById(id);
            if (reservation == null)
            {
                throw new Exception("The Reservation Id is not found");
            }
            return Ok(reservation);
        }


        [HttpPost("BookTicket")]

        public IActionResult BookTicket(Reservation reservation)
        {
            return Ok(reservationRepository.CreateReservation(reservation));
        }


        [HttpPut("UpdateReservation")]
        public ActionResult UpdateReservation(Reservation reservation)
        {
            return Ok(reservationRepository.UpdateReservation(reservation));
        }


        [HttpPut("CancelTicket/{id}")]

        public IActionResult CancelTicket(int id)
        {
            return Ok(reservationRepository.DeleteReservation(id));
        }
    }
}
