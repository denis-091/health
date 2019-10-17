using health.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace health.Services
{
    public class HealthService : IHostedService, IDisposable
    {
        private readonly IServiceProvider _serviceProvider;

        public HealthService(IServiceProvider serviceProvider) {
            _serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
               var _appDbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                var _healthCheckService = scope.ServiceProvider.GetRequiredService<HealthCheckService>();
                var _dataHealthCheckExecution = scope.ServiceProvider.GetRequiredService<IDataHealthCheckExecutionEntries>();

                while (!cancellationToken.IsCancellationRequested)
                {
                    await Task.Delay(20 * 1000, cancellationToken);
                    HealthReport health = await _healthCheckService.CheckHealthAsync();

                    await _dataHealthCheckExecution.CreateHealthCheckExecutionEntries(health);
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

        public void Dispose()
        {
           // throw new NotImplementedException();
        }
    }
}
