using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PKTickets.Interfaces;
using PKTickets.Models;
using PKTickets.Models.DTO;
using PKTickets.Repository;

namespace PKTickets.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShedulesController : ControllerBase
    {

        private readonly IScheduleRepository showRepository;

        public ShedulesController(IScheduleRepository _showRepository)
        {
            showRepository = _showRepository;
        }


        [HttpGet("SchedulesList")]
        public IActionResult SchedulesList()
        {
            return Ok(showRepository.SchedulesList());
        }

        [HttpGet("AvailableSchedulesList")]
        public IActionResult AvailableSchedulesList()
        {
            return Ok(showRepository.AvailableSchedulesList());
        }

        [HttpGet("ScheduleById/{id}")]

        public ActionResult ScheduleById(int id)
        {
            var show = showRepository.ScheduleById(id);
            if (show == null)
            {
                return NotFound();
            }
            return Ok(show);
        }

        [HttpGet("SchedulesByMovieId/{id}")]
        public IActionResult SchedulesByMovieId(int id)
        {
            return Ok(showRepository.SchedulesByMovieId(id));
        }


        [HttpPost("CreateSchedule")]
        public IActionResult CreateSchedule(Schedule show)
        {
            return Ok(showRepository.CreateSchedule(show));
        }


        [HttpPut("UpdateSchedule")]
        public ActionResult UpdateSchedule(Schedule show)
        {
            return Ok(showRepository.UpdateSchedule(show));
        }


        [HttpPut("DeleteSchedule/{id}")]

        public IActionResult DeleteSchedule(int id)
        {
            return Ok(showRepository.DeleteSchedule(id));
        }
    }
}
