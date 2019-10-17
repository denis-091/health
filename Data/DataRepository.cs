using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace health.Data
{
    public class DataRepository : IDataHealthCheckExecutionEntries
    {
        private readonly AppDbContext context;

        public DataRepository(AppDbContext ctx) => context = ctx;

        public async Task CreateHealthCheckExecutionEntries(HealthReport healthReport)
        {
            foreach (var entry in healthReport.Entries)
            {
                await context.HealthCheckExecutionEntries.AddAsync(new HealthCheckExecutionEntries() { 
                    Description = entry.Value.Description,
                    Duration = entry.Value.Duration,
                    Name = entry.Key,
                    Status = entry.Value.Status,
                    DateTime = DateTime.Now
                });

                await context.SaveChangesAsync();
            }
        }

        public IEnumerable<HealthCheckExecutionEntries> GetAllHealthCheckExecutionEntries()
        {
            return context.HealthCheckExecutionEntries;
        }
    }
}
