using System.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Dapper;
using Capstone_project_WorkerService.Models;
using Capstone_project_WorkerService.Contracts;

namespace Capstone_project_WorkerService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IDueDateHandler _handler;
        public WeatherForecastController(ILogger<WeatherForecastController> logger, IDueDateHandler handler)
        {
            _logger = logger;
            _handler = handler;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet("dashboards")]
        public async Task<IActionResult> Dashboards()
        {
            // string sql = "select Id, BranchId, SalesOfMonth, OriginalSales, Profit, PlatformFee, Total, Date, DueDate, Status from Dashboard where DueDate <= @datenow;";
            // var dashboards = await _db.QueryAsync<Dashboard>(sql, new { datenow = DateTime.Now.Date });
            var dashboards = await _handler.GetAll();
            return Ok(dashboards);
        }
    }
}
