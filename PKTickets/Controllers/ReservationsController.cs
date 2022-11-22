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
            return Ok(reservationRepository.ReservationList());
        }

        [HttpGet("ReservationsByScheduleId")]
        public IActionResult ReservationsByScheduleId(int id)
        {
            return Ok(reservationRepository.ReservationsByShowId(id));
        }

        [HttpGet("ReservationById/{id}")]

        public ActionResult ReservationById(int id)
        {
            var reservation = reservationRepository.ReservationById(id);
            if (reservation == null)
            {
               return NotFound();
            }
            return Ok(reservation);
        }


        [HttpPost("BookTicket")]

        public IActionResult BookTicket(ReservationDTO reservation)
        {
            return Ok(reservationRepository.CreateReservation(reservation));
        }


        [HttpPut("UpdateReservation")]
        public ActionResult UpdateReservation(ReservationDTO reservation)
        {
            return Ok(reservationRepository.UpdateReservation(reservation));
        }


        [HttpPut("CancelTicket/{id}")]

        public IActionResult CancelTicket(int id)
        {
            return Ok(reservationRepository.DeleteReservation(id));
        }
        //[HttpGet("ReservationsByUserId")]
        //public IActionResult ReservationsByUserId(int id)
        //{
        //    return Ok(reservationRepository.ReservationsByShowId(id));
        //}
    }
}
