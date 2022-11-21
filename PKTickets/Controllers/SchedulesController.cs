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
            var shows = showRepository.SchedulesList();
            if (shows.Count() == 0)
            {
                Messages messages = new Messages();
                messages.Message = "The Schedule List is empty";
                return Ok(messages.Message);
            }
            return Ok(shows);
        }

        [HttpGet("ScheduleById/{id}")]

        public ActionResult ScheduleById(int id)
        {
            var show = showRepository.ScheduleById(id);
            if (show == null)
            {
                Messages messages = new Messages();
                messages.Message = "The Schedule Id is not found";
                return Ok(messages.Message);
            }
            return Ok(show);
        }

        [HttpGet("SchedulesByMovieId/{id}")]
        public IActionResult SchedulesByMovieId(int id)
        {
            var shows = showRepository.SchedulesByMovieId(id);
            if (shows.Count() == 0)
            {
                Messages messages = new Messages();
                messages.Message = "The given  Id  Schedule list is empty";
                return Ok(messages.Message);
            }
            return Ok(shows);
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
