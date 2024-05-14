using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
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
        public ActionResult<IEnumerable<Flight>> GetAll()
        {
            var flights = _flightService.GetAll();
            return Ok(flights);
        }

        /// <summary>
        /// Zwraca konkretny lot na podstawie jego id.
        /// </summary>
        [HttpGet("{id}")]
        public ActionResult<Flight> GetById([FromRoute] int id)
        {
            Console.WriteLine("GetById: " + id);
            var flight = _flightService.GetById(id);
            return Ok(flight);
        }

        /// <summary>
        /// Pozwala utworzyć nowy lot na podstawie numeru lotu, daty, miejsca wylotu, miejsca przylotu oraz typu samolotu.
        /// </summary>
        /// <remarks>
        /// Wartości numerLotu, miejsceWylotu, miejscePrzylotu oraz typSamolotu nie mogą być puste.
        /// 
        /// Wartość dataWylotuString może być pusta lub null, wtedy system przyjmie obecną datę i godzinę.
        /// 
        /// Wartość typSamolotu musi należeć do tego zbioru: Embraer, Boeing, Airbus. Przyjmuje też wartości: 0, 1, 2.
        ///  
        /// Przykładowe zapytanie:
        ///
        ///     POST /flight
        ///     {
        ///         "numerLotu": 5,
        ///         "dataWylotuString": "16.04.2024 14:30",
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
            Console.WriteLine("Create");
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
            Console.WriteLine("Update: " + id);
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
            Console.WriteLine("Delete: " + id);
            _flightService.Delete(id);
            return NoContent();
        }
    }
}
