using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace health.Models
{
    public class MonitoringServices
    {
        public HealthReport CurrentHealth { get; set; }
        public IEnumerable<ErrorService> LastHourHealth { get; set; }
        public IEnumerable<ErrorService> LastDayHealth { get; set; }
    }
}
