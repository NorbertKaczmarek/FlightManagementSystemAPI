using AutoMapper;
using FlightManagementSystem.Middleware;
using FlightManagementSystem.Models;
using System.Globalization;
using FlightManagementSystem.Entities;

namespace FlightManagementSystem.Services
{
    public interface IFlightService
    {
        IEnumerable<Flight> GetAll();
        Flight GetById(int id);
        int Create(CreateFlightDto dto);
        void Update(int id, CreateFlightDto dto);
        void Delete(int id);
    }

    public class FlightService : IFlightService
    {
        private readonly FlightManagementDbContext _context;

        public FlightService(FlightManagementDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Flight> GetAll()
        {
            Console.WriteLine($"GetAll");
            var flights = _context
                .Flights
                .ToList();

            //if (flights is null || flights.Count == 0) return Array.Empty<Flight>().ToList();

            return flights;
        }

        public Flight GetById(int id)
        {
            Console.WriteLine($"GetById {id}");
            var flight = _context
                .Flights
                .FirstOrDefault(f => f.Id == id);

            if (flight is null) throw new NotFoundException("Flight not found");

            return flight;
        }

        public int Create(CreateFlightDto dto)
        {
            Console.WriteLine($"Create");
            var newFlight = new Flight
            {
                NumerLotu = dto.NumerLotu,
                MiejsceWylotu = dto.MiejsceWylotu,
                MiejscePrzylotu = dto.MiejscePrzylotu,
                TypSamolotu = dto.TypSamolotu,
            };

            newFlight.DataWylotu = TryParseDate(dto.DataWylotuString);

            _context
                .Flights
                .Add(newFlight);

            _context.SaveChanges();

            return newFlight.Id;
        }

        public void Update(int id, CreateFlightDto dto)
        {
            Console.WriteLine($"Update {id}");
            var flight = GetById(id);

            flight.NumerLotu = dto.NumerLotu;
            flight.DataWylotu = TryParseDate(dto.DataWylotuString);
            flight.MiejsceWylotu = dto.MiejsceWylotu;
            flight.MiejscePrzylotu = dto.MiejscePrzylotu;
            flight.TypSamolotu = dto.TypSamolotu;

            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            Console.WriteLine($"Delete {id}");
            var flight = GetById(id);

            _context.Flights.Remove(flight);
            _context.SaveChanges();
        }

        private DateTime TryParseDate(string date)
        {
            if (date == null | date == "")
            {
                return DateTime.Now;
            }
            else
            {
                DateTime DataWylotuParsed;
                string[] formats = { "dd.MM.yyyy HH:mm" };
                if (DateTime.TryParseExact(date, formats, new CultureInfo("pl-PL"), DateTimeStyles.None, out DataWylotuParsed))
                {
                     return DataWylotuParsed;
                }
                else
                {
                    throw new BadRequestException("Invalid date format, should be: dd.MM.yyyy HH:mm");
                }
            }
        }
    }
}
