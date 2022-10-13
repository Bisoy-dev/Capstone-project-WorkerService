using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Capstone_project_WorkerService.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Capstone_project_WorkerService.Workers
{
    public class StatusWorker : BackgroundService
    {
        private readonly ILogger<StatusWorker> _logger;
        private readonly IDueDateHandler _handler;
        public StatusWorker(ILogger<StatusWorker> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _handler = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<IDueDateHandler>();
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                // var dashboards = await _handler.GetAll();
                // _logger.LogInformation($"{dashboards.FirstOrDefault().BranchId} {dashboards.FirstOrDefault().SalesOfMonth}");
                // await Task.Delay(3000);
                // _logger.LogInformation($"{dashboards.LastOrDefault().BranchId} {dashboards.LastOrDefault().SalesOfMonth}");
                // await Task.Delay(3000);
                // deactivate store who did not pay 5 days after the due date
                var deactivateStoreResult = await _handler.DeActivate5DaysUnpaidStore();
                _logger.LogInformation($"Number of stores being deactivate {deactivateStoreResult}");

                var affectedRows = await _handler.UpdateStatus();
                _logger.LogInformation(affectedRows.ToString());
                await Task.Delay(3000);
            }
        }
    }
}