using Microsoft.EntityFrameworkCore;

namespace health.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options) { }

        public DbSet<HealthCheckExecutionEntries> HealthCheckExecutionEntries { get; set; }
    }
}
