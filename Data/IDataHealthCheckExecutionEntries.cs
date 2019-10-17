using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace health.Data
{
    public interface IDataHealthCheckExecutionEntries
    {
        Task CreateHealthCheckExecutionEntries(HealthReport healthReport);
        IEnumerable<HealthCheckExecutionEntries> GetAllHealthCheckExecutionEntries();
    }
}
