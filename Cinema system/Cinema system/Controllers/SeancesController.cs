using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using CinemaSystem.Interfaces;
using CinemaSystem.Models;

namespace CinemaSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeancesController : ControllerBase
    {

        private readonly ISeanceService _seanceService;

        private const string ErrorOfSeanceNonexistence = "This seance does not exists.";
        private const string ErrorOfSeanceExistence = "This seance is already exists.";
        private const string ErrorOfHallNonexistence = "This hall does not exists.";
        private const string ErrorOfNonmatchingSeanceTime = "At the specified time has already been assigned a session.";
        private const string ErrorOfMovieNonexistence = "Such a movie is not shown at this time.";
        private const string ErrorOfSeanceTime = "Invalid seance time specified.";

        public SeancesController(ISeanceService seanceService)
        {
            _seanceService = seanceService;
        }

        [HttpGet]
        public IActionResult GetSeances([FromQuery] SeanceFilter filter)
        {
            var seances = _seanceService.GetSeances(filter);

            return Ok(new GetResponse<IEnumerable<SeanceView>>(seances));
        }

        [HttpGet("{seanceId}")]
        public IActionResult GetSeance(int seanceId)
        {
            if (!_seanceService.CheckSeanceExists(seanceId))
            {
                return NotFound(new Response(ErrorOfSeanceNonexistence));
            }

            var seance = _seanceService.GetSeance(seanceId);

            return Ok(new GetResponse<SeanceFullView>(seance));
        }

        [HttpPost]
        public IActionResult CreateSeance([FromBody] SeanceInfo seance)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new Response(ModelState));
            }

            if (_seanceService.CheckHallExists(seance.HallId))
            {
                return BadRequest(new Response(ErrorOfHallNonexistence));
            }

            if (_seanceService.CheckCorrectSeanceTime(seance))
            {
                return BadRequest(new Response(ErrorOfSeanceTime));
            }

            if (_seanceService.CheckSeanceExists(seance))
            {
                return BadRequest(new Response(ErrorOfSeanceExistence));
            }

            if (_seanceService.СheckForNonmatchingSeanceTime(seance))
            {
                return BadRequest(new Response(ErrorOfNonmatchingSeanceTime));
            }

            if (_seanceService.CheckCorrectMovie(seance))
            {
                return BadRequest(new Response(ErrorOfMovieNonexistence));
            }

            int id = _seanceService.CreateSeance(seance);

            var response = new CreateResponse(id);

            return CreatedAtAction(nameof(GetSeance), new { id = response.Id }, response);
        }

        [HttpPut("{seanceId}")]
        public IActionResult EditSeance([FromBody] SeanceInfo seance, int seanceId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new Response(ModelState));
            }

            if (!_seanceService.CheckSeanceExists(seanceId))
            {
                return NotFound(new Response(ErrorOfSeanceNonexistence));
            }

            if (_seanceService.CheckCorrectSeanceTime(seance))
            {
                return BadRequest(new Response(ErrorOfSeanceTime));
            }

            if (_seanceService.СheckForNonmatchingSeanceTime(seance, seanceId))
            {
                return BadRequest(new Response(ErrorOfNonmatchingSeanceTime));
            }

            if (_seanceService.CheckCorrectMovie(seance))
            {
                return BadRequest(new Response(ErrorOfMovieNonexistence));
            }

            _seanceService.EditSeance(seanceId, seance);

            return Ok(new Response());
        }

        [HttpDelete("{seanceId}")]
        public IActionResult DeleteMovie(int seanceId)
        {
            if (!_seanceService.CheckSeanceExists(seanceId))
            {
                return NotFound(new Response(ErrorOfSeanceNonexistence));
            }

            _seanceService.DeleteSeance(seanceId);

            return Ok(new Response());
        }

    }
}