using Entities.Models;
using System.Data.Entity;

namespace Entities
{
    public class FlightsDBContext : DbContext
    {
        public FlightsDBContext() : base("FlightsDBContext")
        {
            Database.SetInitializer(new FlightsInitializer());
        }

        public DbSet<Flight> Flights { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Reservation> Reservations { get; set; }
    }
}
