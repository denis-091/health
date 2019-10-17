using health.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace health.Models
{
    public class ErrorService
    {
        public string Name { get; set; }
        public TimeSpan MaxDuration { get; set; }
        public int Total { get; set; }
        public IEnumerable<HealthCheckExecutionEntries> Data { get; set; }
    }
}
