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
    public class ShowsController : ControllerBase
    {

        private readonly IShowRepository showRepository;

        public ShowsController(IShowRepository _showRepository)
        {
            showRepository = _showRepository;
        }


        [HttpGet("ShowsList")]
        public IActionResult ShowsList()
        {
            var shows = showRepository.ShowsList();
            if (shows.Count() == 0)
            {
                throw new Exception("The Show list is empty");
            }
            return Ok(shows);
        }

        [HttpGet("ShowById/{id}")]

        public ActionResult ShowById(int id)
        {
            var show = showRepository.ShowById(id);
            if (show == null)
            {
                throw new Exception("The Show Id is not found");
            }
            return Ok(show);
        }

        [HttpGet("ShowsByMovieId/{id}")]
        public IActionResult ShowsByMovieId(int id)
        {
            var shows = showRepository.ShowsByMovieId(id);
            if (shows.Count() == 0)
            {
                throw new Exception("The given name of Show list is empty");
            }
            return Ok(shows);
        }
        [HttpPost("CreateShow")]

        public IActionResult CreateShow(Show show)
        {
            return Ok(showRepository.CreateShow(show));
        }


        [HttpPut("UpdateShow")]
        public ActionResult UpdateShow(Show show)
        {
            return Ok(showRepository.UpdateShow(show));
        }


        [HttpPut("DeleteShow/{id}")]

        public IActionResult DeleteShow(int id)
        {
            return Ok(showRepository.DeleteShow(id));
        }
    }
}
