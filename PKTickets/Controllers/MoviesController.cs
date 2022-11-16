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
            var movies = movieRepository.GetAllMovies();
            if (movies.Count() == 0)
            {
                throw new Exception("The Movie list is empty");
            }
            return Ok(movies);
        }
        
        [HttpGet("GetMoviesByTitle/{title}")]
        public IActionResult GetMoviesByTitle(string title)
        {
            var movies = movieRepository.MovieByTitle(title);
            if (movies.Count() == 0)
            {
                throw new Exception("The Movie Title list is empty");
            }
            return Ok(movies);
        }

        [HttpGet("GetMoviesByGenre/{Genre}")]
        public IActionResult MovieByGenre(string title)
        {
            var movies = movieRepository.MovieByTitle(title);
            if (movies.Count() == 0)
            {
                throw new Exception("The Movie Genre list is empty");
            }
            return Ok(movies);
        }

        [HttpGet("MovieById/{movieId}")]

        public ActionResult MovieById(int movieId)
        {
            var movie = movieRepository.MovieById(movieId);
            if (movie == null)
            {
                throw new Exception("The Movie Id is not found");
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

        [HttpPost("DubMovie")]
        public ActionResult DubMovie(DubDTO movie)
        {
            return Ok(movieRepository.DubMovie(movie));
        }

        [HttpPut("DubMovieUpdate")]
        public ActionResult DubMovieUpdate(Movie movie)
        {
            return Ok(movieRepository.DubMovieUpdate(movie));
        }
    }
}
