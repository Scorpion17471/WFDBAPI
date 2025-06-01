using Microsoft.EntityFrameworkCore;

namespace WFDBAPI.Database.Entities.Context
{
    public class PrimeDBContext : DbContext
    {
        public PrimeDBContext(DbContextOptions<PrimeDBContext> options) : base(options)
        {
        }

        // Database Model Link
        public DbSet<Relic> Relic { get; set; }
        public DbSet<Warframe> Warframe { get; set; }
        public DbSet<Reward> Reward { get; set; }
    }
}
