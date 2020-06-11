using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using CinemaSystem.Interfaces;
using CinemaSystem.Models;

namespace CinemaSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CinemasController : ControllerBase
    {

        private readonly ICinemaService _сinemaService;

        private const string ErrorOfCinemaNonexistence = "This cinema does not exists.";
        private const string ErrorOfCinemaExistence = "This cinema is already exists.";
        private const string ErrorOfHallNonexistence = "This hall does not exist in this cinema.";
        private const string ErrorOfHallExistence = "This hall is already exists in this cinema.";

        public CinemasController(ICinemaService сinemaService)
        {
            _сinemaService = сinemaService;
        }

        [HttpGet]
        public IActionResult GetCinemas()
        {
            var cinemas = _сinemaService.GetCinemas();

            return Ok(new GetResponse<IEnumerable<CinemaView>>(cinemas));
        }

        [HttpGet("{cinemaId}")]
        public IActionResult GetCinema(int cinemaId)
        {
            if (!_сinemaService.CheckCinemaExists(cinemaId))
            {
                return NotFound(new Response(ErrorOfCinemaNonexistence));
            }

            var cinema = _сinemaService.GetCinema(cinemaId);

            return Ok(new GetResponse<CinemaView>(cinema));
        }

        [HttpPost]
        public IActionResult CreateCinema([FromBody] CinemaView cinema)
        {
            if (_сinemaService.CheckCinemaExists(cinema))
            {
                return BadRequest(new Response(ErrorOfCinemaExistence));
            }
            
            if (!ModelState.IsValid)
            {
                return BadRequest(new Response(ModelState));
            }

            int id = _сinemaService.CreateCinema(cinema);

            var response = new CreateResponse(id);

            return CreatedAtAction(nameof(GetCinema), new { id = response.Id }, response);
        }

        [HttpPut("{cinemaId}")]
        public IActionResult EditCinema([FromBody] CinemaView cinema, int cinemaId)
        {
            if (!_сinemaService.CheckCinemaExists(cinemaId))
            {
                return NotFound(new Response(ErrorOfCinemaNonexistence));
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(new Response(ModelState));
            }

            _сinemaService.EditCinema(cinemaId, cinema);

            return Ok(new Response());
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCinema(int cinemaId)
        {
            if (!_сinemaService.CheckCinemaExists(cinemaId))
            {
                return NotFound(new Response(ErrorOfCinemaNonexistence));
            }

            _сinemaService.DeleteCinema(cinemaId);

            return Ok(new Response());
        }

        [HttpGet("{cinemaId}/halls")]
        public IActionResult GetHalls(int cinemaId)
        {
            if (!_сinemaService.CheckCinemaExists(cinemaId))
            {
                return NotFound(new Response(ErrorOfCinemaNonexistence));
            }

            var halls = _сinemaService.GetHalls(cinemaId);

            return Ok(new GetResponse<IEnumerable<HallView>>(halls));
        }

        [HttpGet("{cinemaId}/halls/{hallId}")]
        public IActionResult GetHall(int cinemaId, int hallId)
        {
            if (!_сinemaService.CheckCinemaExists(cinemaId))
            {
                return NotFound(new Response(ErrorOfCinemaNonexistence));
            }

            if (!_сinemaService.CheckHallExists(cinemaId, hallId))
            {
                return NotFound(new Response(ErrorOfHallNonexistence));
            }

            var hall = _сinemaService.GetHall(hallId);

            return Ok(new GetResponse<HallFullView>(hall));
        }

        [HttpPost("{cinemaId}/halls")]
        public IActionResult CreateHall([FromBody] HallInfo hall, int cinemaId)
        {
            if (!_сinemaService.CheckCinemaExists(cinemaId))
            {
                return NotFound(new Response(ErrorOfCinemaNonexistence));
            }

            if (_сinemaService.CheckHallExists(cinemaId, hall))
            {
                return NotFound(new Response(ErrorOfHallExistence));
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(new Response(ModelState));
            }

            var id = _сinemaService.CreateHall(cinemaId, hall);

            var response = new CreateResponse(id);

            return CreatedAtAction(nameof(GetHall), new { cinemaId, hallId = response.Id }, response);
        }

        [HttpPut("{cinemaId}/halls/{hallId}")]
        public IActionResult EditHall([FromBody] HallInfo hall, int cinemaId, int hallId)
        {
            if (!_сinemaService.CheckCinemaExists(cinemaId))
            {
                return NotFound(new Response(ErrorOfCinemaNonexistence));
            }

            if (!_сinemaService.CheckHallExists(cinemaId, hallId))
            {
                return NotFound(new Response(ErrorOfHallNonexistence));
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(new Response(ModelState));
            }

            _сinemaService.EditHall(hallId, hall);

            return Ok(new Response());
        }

        [HttpDelete("{cinemaId}/halls/{hallId}")]
        public IActionResult DeleteHall(int cinemaId, int hallId)
        {
            if (!_сinemaService.CheckCinemaExists(cinemaId))
            {
                return NotFound(new Response(ErrorOfCinemaNonexistence));
            }

            if (!_сinemaService.CheckHallExists(cinemaId, hallId))
            {
                return NotFound(new Response(ErrorOfHallNonexistence));
            }

            _сinemaService.DeleteHall(hallId);

            return Ok(new Response());
        }

    }
}