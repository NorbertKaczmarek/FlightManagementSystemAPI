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
            var flights = _context
                .Flights
                .ToList();

            return flights;
        }

        public Flight GetById(int id)
        {
            var flight = _context
                .Flights
                .FirstOrDefault(f => f.Id == id);

            if (flight is null) throw new NotFoundException("Flight not found");

            return flight;
        }

        public int Create(CreateFlightDto dto)
        {
            var newFlight = new Flight
            {
                NumerLotu = dto.NumerLotu,
                DataWylotu = dto.DataWylotu,
                MiejsceWylotu = dto.MiejsceWylotu,
                MiejscePrzylotu = dto.MiejscePrzylotu,
                TypSamolotu = dto.TypSamolotu,
            };

            _context
                .Flights
                .Add(newFlight);

            _context.SaveChanges();

            return newFlight.Id;
        }

        public void Update(int id, CreateFlightDto dto)
        {
            var flight = GetById(id);

            flight.NumerLotu = dto.NumerLotu;
            flight.DataWylotu = dto.DataWylotu;
            flight.MiejsceWylotu = dto.MiejsceWylotu;
            flight.MiejscePrzylotu = dto.MiejscePrzylotu;
            flight.TypSamolotu = dto.TypSamolotu;

            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var flight = GetById(id);

            _context.Flights.Remove(flight);
            _context.SaveChanges();
        }
    }
}
