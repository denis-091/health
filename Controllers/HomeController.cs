using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using health.Models;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using health.Data;
using Microsoft.Extensions.Configuration;

namespace health.Controllers
{
    public class HomeController : Controller
    {
        private readonly HealthCheckService _healthCheckService;
        private readonly ILogger<HomeController> _logger;
        private readonly IDataHealthCheckExecutionEntries _dataHealthCheckExecution;
        private readonly IConfiguration _configuration;

        public HomeController(ILogger<HomeController> logger, HealthCheckService healthCheckService,
            IDataHealthCheckExecutionEntries dataHealthCheckExecution, IConfiguration configuration)
        {
            _logger = logger;
            _healthCheckService = healthCheckService;
            _dataHealthCheckExecution = dataHealthCheckExecution;
            _configuration = configuration;
        }

        public async Task<IActionResult> Index() 
        {
            ViewBag.EvaluationTimeOnSeconds = int.Parse(_configuration["HealthChecksUI:EvaluationTimeOnSeconds"]);

            var _lastHour = _dataHealthCheckExecution.GetAllHealthCheckExecutionEntries()
                .Where(s => s.DateTime.Hour == DateTime.Now.Hour)
                .GroupBy(n => n.Name)
                .Select(s => new ErrorService
                {
                    Name = s.Key,
                    Total = s.Count(x => x.Status != HealthStatus.Healthy),
                    MaxDuration = s.Select(a => a.Duration).Max(),
                    Data = s
                });

            var _lastDay = _dataHealthCheckExecution.GetAllHealthCheckExecutionEntries()
                .Where(s => s.DateTime.Day == DateTime.Now.Day)
                .GroupBy(n => n.Name)
                .Select(s => new ErrorService
                {
                    Name = s.Key,
                    Total = s.Count(x => x.Status != HealthStatus.Healthy),
                    MaxDuration = s.Select(a => a.Duration).Max(),
                    Data = s
                });

            HealthReport _health = await _healthCheckService.CheckHealthAsync();

            MonitoringServices report = new MonitoringServices() { 
                CurrentHealth = await _healthCheckService.CheckHealthAsync(),
                LastDayHealth = _lastDay,
                LastHourHealth = _lastHour
            };

            return View(report);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
