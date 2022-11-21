using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PKTickets.Interfaces;
using PKTickets.Models;

namespace PKTickets.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TheatersController : ControllerBase
    {

        private readonly ITheaterRepository theaterRepository;

        public TheatersController(ITheaterRepository _theaterRepository)
        {
            theaterRepository = _theaterRepository;
        }


        [HttpGet("GetTheaterList")]
        public IActionResult GetTheaterList()
        {
            var theaters = theaterRepository.GetTheaters();
            if (theaters.Count() == 0)
            {
                throw new Exception("The Theater list is empty");
            }
            return Ok(theaters);
        }

        [HttpGet("TheaterById/{theaterId}")]

        public ActionResult TheaterById(int theaterId)
        {
            var theater = theaterRepository.TheaterById(theaterId);
            if (theater == null)
            {
                throw new Exception("The Theater Id is not found");
            }
            return Ok(theater);
        }

        [HttpGet("TheatersByLocation/{location}")]

        public ActionResult TheatersByLocation(string location)
        {
            var theaters = theaterRepository.TheaterByLocation(location);
            if (theaters.Count() == 0)
            {
                throw new Exception("There is no Theater in that Location");
            }
            return Ok(theaters);
        }


        [HttpPost("CreateTheater")]

        public IActionResult CreateTheater(Theater theater)
        {
            return Ok(theaterRepository.CreateTheater(theater));
        }


        [HttpPut("UpdateTheater")]
        public ActionResult UpdateTheater(Theater theater)
        {
            return Ok(theaterRepository.UpdateTheater(theater));
        }


        [HttpPut("DeleteTheater/{theaterId}")]

        public IActionResult DeleteTheater(int theaterId)
        {
            return Ok(theaterRepository.DeleteTheater(theaterId));
        }
    }
}
