using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FlightManagementSystem.Entities;
using FlightManagementSystem.Models;
using FlightManagementSystem.Services;

namespace FlightManagementSystem.Controllers
{
    [ApiController]
    [Route("flight")]
    public class FlightController : ControllerBase
    {
        private readonly IFlightService _flightService;

        public FlightController(IFlightService flightService)
        {
            _flightService = flightService;
        }

        /// <summary>
        /// Zwraca wszystkie utworzone loty.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Flight>>> GetAll()
        {
            var flights = await _flightService.GetAll();
            return Ok(flights);
        }

        /// <summary>
        /// Zwraca konkretny lot na podstawie jego id.
        /// </summary>
        [HttpGet("{id}")]
        public ActionResult<Flight> GetById([FromRoute] int id)
        {
            var flight = _flightService.GetById(id);
            return Ok(flight);
        }

        /// <summary>
        /// Pozwala utworzyć nowy lot na podstawie numeru lotu, daty, miejsca wylotu, miejsca przylotu oraz typu samolotu.
        /// </summary>
        /// <remarks>
        /// Wartości numerLotu, dataWylotu miejsceWylotu, miejscePrzylotu oraz typSamolotu nie mogą być puste.
        /// 
        /// Wartość typSamolotu musi należeć do tego zbioru: Embraer, Boeing, Airbus. Przyjmuje też wartości: 0, 1, 2.
        ///  
        /// Przykładowe zapytanie:
        ///
        ///     POST /flight
        ///     {
        ///         "numerLotu": 5,
        ///         "dataWylotu": "2024-05-10T06:30:00",
        ///         "miejsceWylotu": "Warszawa",
        ///         "miejscePrzylotu": "Poznań",
        ///         "typSamolotu": "Embraer"
        ///     }
        ///     
        /// </remarks>
        [Authorize]
        [HttpPost]
        public ActionResult Create([FromBody] CreateFlightDto dto)
        {
            var id = _flightService.Create(dto);
            return Created($"/flight/{id}", null);
        }

        /// <summary>
        /// Pozwala modyfikować utworzony wcześniej lot na podstawie jego id.
        /// </summary>
        [Authorize]
        [HttpPost("{id}")]
        public ActionResult Update([FromRoute] int id, [FromBody] CreateFlightDto dto)
        {
            _flightService.Update(id, dto);
            return Ok();
        }

        /// <summary>
        /// Pozwala usunąć lot na podstawie jego id.
        /// </summary>
        [Authorize]
        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] int id)
        {
            _flightService.Delete(id);
            return NoContent();
        }
    }
}
