using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PKTickets.Interfaces;
using PKTickets.Models;
using PKTickets.Models.DTO;

namespace PKTickets.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScreensController : ControllerBase
    {

        private readonly IScreenRepository screenRepository;

        public ScreensController(IScreenRepository _screenRepository)
        {
            screenRepository = _screenRepository;
        }


        [HttpGet("GetScreensList")]
        public IActionResult GetScreensList()
        {
            return Ok(screenRepository.GetAllScreens());
        }

        [HttpGet("ScreenById/{screenId}")]

        public ActionResult ScreenById(int screenId)
        {
            var screen = screenRepository.ScreenById(screenId);
            if (screen == null)
            {
               return NotFound();
            }
            return Ok(screen);
        }


        [HttpPost("CreateScreen")]

        public IActionResult CreateScreen(Screen screen)
        {
            return Ok(screenRepository.AddScreen(screen));
        }


        [HttpPut("UpdateScreen")]
        public ActionResult UpdateScreen(Screen screen)
        {
            return Ok(screenRepository.UpdateScreen(screen));
        }


        [HttpPut("DeleteScreen/{screenId}")]

        public IActionResult DeleteScreen(int screenId)
        {
            return Ok(screenRepository.RemoveScreen(screenId));
        }

        [HttpGet("GetScreensByTheaterId/{id}")]
        public IActionResult GetScreensByTheaterId(int id)
        {
            return Ok(screenRepository.TheaterScreens(id));
        }

    }
}
