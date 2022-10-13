using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Capstone_project_WorkerService.Contracts;
using Capstone_project_WorkerService.Workers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Capstone_project_WorkerService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) => 
                {
                    var connectionString = context.Configuration.GetConnectionString("FastlineDbConnection");
                    services.AddScoped<IDbConnection, SqlConnection>(option => 
                    {
                        return new SqlConnection(connectionString);
                    });
                    services.AddScoped<IDueDateHandler, DueDateHandler>();
                    services.AddHostedService<StatusWorker>();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
