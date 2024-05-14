using Microsoft.EntityFrameworkCore;

namespace FlightManagementSystem.Entities
{
    public class FlightManagementDbContext : DbContext
    {
        public FlightManagementDbContext(DbContextOptions<FlightManagementDbContext> options) : base(options) { }

        public DbSet<Flight> Flights { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
