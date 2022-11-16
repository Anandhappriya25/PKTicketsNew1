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
            var screens = screenRepository.GetAllScreens();
            if (screens.Count() == 0)
            {
                throw new Exception("The Screens list is empty");
            }
            return Ok(screens);
        }

        [HttpGet("ScreenById/{screenId}")]

        public ActionResult ScreenById(int screenId)
        {
            var screen = screenRepository.ScreenById(screenId);
            if (screen == null)
            {
                throw new Exception("The Screen Id is not found");
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

        [HttpGet("GetScreensByTheaterId")]
        public IActionResult GetScreensByTheaterId(int id)
        {
            var screens = screenRepository.TheaterScreens(id);
            if (screens.Count() == 0)
            {
                throw new Exception("This Theater Does Not added any Screens yet");
            }
            return Ok(screens);
        }

    }
}
