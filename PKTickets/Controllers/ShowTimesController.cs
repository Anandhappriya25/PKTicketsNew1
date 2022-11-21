using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PKTickets.Interfaces;
using PKTickets.Models.DTO;
using PKTickets.Models;
using PKTickets.Repository;

namespace PKTickets.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShowTimesController : ControllerBase
    {
        private readonly IShowTimeRepository showTimeRepository;

        public ShowTimesController(IShowTimeRepository _showTimeRepository)
        {
            showTimeRepository = _showTimeRepository;
        }

        [HttpGet("GetAllShowTimes")]
        public IActionResult GetAllShowTimes()
        {
            return Ok(showTimeRepository.GetAllShowTimes();
        }

        [HttpGet("ShowTimeById/{showTimeId}")]

        public ActionResult ShowTimeById(int showTimeId)
        {
            var showTime = showTimeRepository.ShowTimeasStringById(showTimeId);
            if (showTime == null)
            {
                return NotFound();
            }
            return Ok(showTime);
        }

        [HttpPost("AddShowTiming")]

        public IActionResult AddShowTiming(ShowTimeDTO showTimeDTO)
        {
            return Ok(showTimeRepository.CreateShowTime(showTimeDTO));
        }


        [HttpPut("UpdateShowTime")]
        public ActionResult UpdateShowTime(ShowTimeDTO showTimeDTO)
        {
            return Ok(showTimeRepository.UpdateShowTime(showTimeDTO));
        }


        [HttpPut("DeleteShowTime/{showTimeId}")]

        public IActionResult DeleteShowTime(int showTimeId)
        {
            return Ok(showTimeRepository.DeleteShowTime(showTimeId));
        }

    }
}
