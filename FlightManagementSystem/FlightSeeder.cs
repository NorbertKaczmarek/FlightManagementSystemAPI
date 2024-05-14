using FlightManagementSystem.Entities;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace FlightManagementSystem
{
    public class FlightSeeder
    {
        private readonly FlightManagementDbContext _dbContext;

        public FlightSeeder(FlightManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Seed()
        {
            if (_dbContext.Database.CanConnect())
            {
                if (_dbContext.Database.IsRelational())
                {
                    var pendingMigrations = _dbContext.Database.GetPendingMigrations();
                    if (pendingMigrations != null && pendingMigrations.Any())
                    {
                        _dbContext.Database.Migrate();
                    }
                }

                if (!_dbContext.Flights.Any())
                {
                    var flights = GetFlights();
                    _dbContext.Flights.AddRange(flights);
                    _dbContext.SaveChanges();
                }
            }
        }

        private IEnumerable<Flight> GetFlights()
        {
            var flights = new List<Flight>()
            {
                new Flight()
                {
                    NumerLotu = 1,
                    DataWylotu = new DateTime(2004, 12, 5),
                    MiejsceWylotu = "Warszawa",
                    MiejscePrzylotu = "Gdańsk",
                    TypSamolotu = PlaneType.Boeing
                },
                new Flight()
                {
                    NumerLotu = 2,
                    DataWylotu = new DateTime(2004, 12, 6),
                    MiejsceWylotu = "Gdańsk",
                    MiejscePrzylotu = "Warszawa",
                    TypSamolotu = PlaneType.Boeing
                },
                new Flight()
                {
                    NumerLotu = 3,
                    DataWylotu = new DateTime(2004, 12, 5),
                    MiejsceWylotu = "Poznań",
                    MiejscePrzylotu = "Wrocław",
                    TypSamolotu = PlaneType.Embraer
                },
            };

            return flights;
        }
    }
}
