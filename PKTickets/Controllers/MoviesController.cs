using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PKTickets.Interfaces;
using PKTickets.Models;
using PKTickets.Models.DTO;

namespace PKTickets.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {

        private readonly IMovieRepository movieRepository;

        public MoviesController(IMovieRepository _movieRepository)
        {
            movieRepository = _movieRepository;
        }


        [HttpGet("GetMoviesList")]
        public IActionResult GetMoviesList()
        {
            return Ok(movieRepository.GetAllMovies());
        }

        [HttpGet("GetMoviesByTitle/{title}")]
        public IActionResult GetMoviesByTitle(string title)
        {
            return Ok(movieRepository.MovieByTitle(title));
        }

        [HttpGet("GetMoviesByGenre/{Genre}")]
        public IActionResult MovieByGenre(string Genre)
        {
            return Ok(movieRepository.MovieByGenre(Genre));
        }

        [HttpGet("MovieById/{movieId}")]

        public ActionResult MovieById(int movieId)
        {
            var movie = movieRepository.MovieById(movieId);
            if (movie == null)
            {
                return NotFound();
            }
            return Ok(movie);
        }


        [HttpPost("CreateMovie")]

        public IActionResult CreateMovie(Movie movie)
        {
            return Ok(movieRepository.CreateMovie(movie));
        }


        [HttpPut("UpdateMovie")]
        public ActionResult UpdateMovie(Movie movie)
        {
            return Ok(movieRepository.UpdateMovie(movie));
        }


        [HttpPut("DeleteMovie/{movieId}")]

        public IActionResult DeleteMovie(int movieId)
        {
            return Ok(movieRepository.DeleteMovie(movieId));
        }
    }
}
