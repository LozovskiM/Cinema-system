using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using CinemaSystem.Interfaces;
using CinemaSystem.Models;

namespace CinemaSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {

        private readonly IMovieService _movieService;

        private const string ErrorOfMovieNonexistence = "This movie does not exists.";
        private const string ErrorOfMovieExistence = "This movie is already exists.";
        private const string ErrorOfMovieDate = "Ending date must be later than release date";

        public MoviesController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        [HttpGet]
        public IActionResult GetMovies()
        {
            var movies = _movieService.GetMovies();

            return Ok(new GetResponse<IEnumerable<MovieView>>(movies));
        }

        [HttpGet("{movieId}")]
        public IActionResult GetMovie(int movieId)
        {
            if (!_movieService.CheckMovieExists(movieId))
            {
                return NotFound(new Response(ErrorOfMovieNonexistence));
            }

            var movie = _movieService.GetMovie(movieId);

            return Ok(new GetResponse<MovieFullView>(movie));
        }

        [HttpPost]
        public IActionResult CreateMovie([FromBody] MovieFullView movie)
        {
            if (_movieService.CheckMovieExists(movie))
            {
                return BadRequest(new Response(ErrorOfMovieExistence));
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(new Response(ModelState));
            }

            if (_movieService.CheckMovieDate(movie))
            {
                return BadRequest(new Response(ErrorOfMovieDate));
            }

            int id = _movieService.CreateMovie(movie);

            var response = new CreateResponse(id);

            return CreatedAtAction(nameof(GetMovie), new { id = response.Id }, response);
        }

        [HttpPut("{movieId}")]
        public IActionResult EditMovie([FromBody] MovieFullView movie, int movieId)
        {
            if (!_movieService.CheckMovieExists(movieId))
            {
                return NotFound(new Response(ErrorOfMovieNonexistence));
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(new Response(ModelState));
            }

            if (_movieService.CheckMovieDate(movie))
            {
                return BadRequest(new Response(ErrorOfMovieDate));
            }

            _movieService.EditMovie(movie, movieId);

            return Ok(new Response());
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteMovie(int movieId)
        {
            if (!_movieService.CheckMovieExists(movieId))
            {
                return NotFound(new Response(ErrorOfMovieNonexistence));
            }

            _movieService.DeleteMovie(movieId);

            return Ok(new Response());
        }

    }
}
