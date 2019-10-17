using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace health.Data
{
    public class HealthCheckExecutionEntries
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public HealthStatus Status { get; set; }
        public string Description { get; set; }
        public TimeSpan Duration { get; set; }
        public DateTime DateTime { get; set; }
    }
}
