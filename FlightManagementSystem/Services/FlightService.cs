using AutoMapper;
using FlightManagementSystem.Middleware;
using FlightManagementSystem.Models;
using System.Globalization;
using FlightManagementSystem.Entities;
using Microsoft.EntityFrameworkCore;

namespace FlightManagementSystem.Services
{
    public interface IFlightService
    {
        Task<IEnumerable<Flight>> GetAll();
        Flight GetById(int id);
        int Create(FlightCreateDto dto);
        void Update(int id, FlightEditDto dto);
        void Delete(int id);
    }

    public class FlightService : IFlightService
    {
        private readonly FlightManagementDbContext _context;

        public FlightService(FlightManagementDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Flight>> GetAll()
        {
            //await Console.Out.WriteLineAsync("GetAll");
            var flights = await _context
                .Flights
                .ToListAsync();

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

        public int Create(FlightCreateDto dto)
        {
            var NumerLotuInUse = _context.Flights.Any(u => u.NumerLotu == dto.NumerLotu);

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

        public void Update(int id, FlightEditDto dto)
        {
            var flight = GetById(id);

            var NumerLotuInUse = _context.Flights.FirstOrDefault(u => u.NumerLotu == dto.NumerLotu);

            if (!(NumerLotuInUse == null) && !(NumerLotuInUse.Id == flight.Id)) throw new BadRequestException("NumerLotu already exists.");

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
