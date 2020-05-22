using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using CinemaSystem.Interfaces;
using CinemaSystem.Models;
using System.Linq;

namespace CinemaSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CinemasController : ControllerBase
    {

        private readonly ICinemaService _сinemaService;

        public CinemasController(ICinemaService сinemaService)
        {
            _сinemaService = сinemaService;
        }

        [HttpGet]
        public IActionResult GetCinemas()
        {
            var response = new GetResponse<IEnumerable<CinemaView>>
            {
                IsSuccessful = true,
                RequestedData = _сinemaService.GetCinemas()
            };

            return Ok(response);
        }

        [HttpGet("{cinemaId}")]
        public IActionResult GetCinema(int cinemaId)
        {
            var response = new GetResponse<CinemaView>();

            if (!_сinemaService.IsCinemaExists(cinemaId))
            {
                response.IsSuccessful = false;
                response.ErrorDescriptions.Add("This cinema does not exists.");
                return NotFound(response);
            }

            response.IsSuccessful = true;
            response.RequestedData = _сinemaService.GetCinema(cinemaId);
            
            return Ok(response);
        }

        [HttpPost]
        public IActionResult CreateCinema([FromBody] CinemaView cinema)
        {
            var response = new CreateResponse();

            if (_сinemaService.IsCinemaExists(cinema))
            {
                response.IsSuccessful = false;
                response.ErrorDescriptions.Add("This cinema is already exists.");
                return BadRequest(response);
            }
            
            if (!ModelState.IsValid)
            {
                IEnumerable<string> messages = ModelState.Values
                    .SelectMany(x => x.Errors)
                    .Select(x => x.ErrorMessage);

                response.IsSuccessful = false;
                response.ErrorDescriptions.AddRange(messages);

                return BadRequest(response);
            }

            response.IsSuccessful = true;
            response.Id = _сinemaService.CreateCinema(cinema);

            return CreatedAtAction(nameof(GetCinema), new { id = response.Id }, response);
        }

        [HttpPut("{cinemaId}")]
        public IActionResult EditCinema([FromBody] CinemaView cinema, int cinemaId)
        {
            var response = new Response();

            if (!_сinemaService.IsCinemaExists(cinemaId))
            {
                response.IsSuccessful = false;
                response.ErrorDescriptions.Add("This cinema does not exists.");
                return NotFound(response);
            }

            if (!ModelState.IsValid)
            {
                IEnumerable<string> messages = ModelState.Values
                    .SelectMany(x => x.Errors)
                    .Select(x => x.ErrorMessage);

                response.IsSuccessful = false;
                response.ErrorDescriptions.AddRange(messages);

                return BadRequest(response);
            }

            _сinemaService.EditCinema(cinemaId, cinema);

            response.IsSuccessful = true;

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCinema(int cinemaId)
        {
            var response = new Response();

            if (!_сinemaService.IsCinemaExists(cinemaId))
            {
                response.IsSuccessful = false;
                response.ErrorDescriptions.Add("This cinema does not exists.");
                return NotFound(response);
            }

            _сinemaService.DeleteCinema(cinemaId);

            response.IsSuccessful = true;

            return Ok(response);
        }

    }
}