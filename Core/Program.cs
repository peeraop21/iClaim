
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core
{
    public class Program
    {
        private const string LogPath = "Logs";
        private const string SeqURL = "http://localhost:5341";
        private const string SeqAPIKey = "HOWmCVf6803bLsl8IrmP";
        public static void Main(string[] args)
        {
            string logPath = string.Format(@"{0}\log.txt", LogPath);

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.File(logPath, rollingInterval: RollingInterval.Hour)
                .WriteTo.Seq(SeqURL, apiKey: SeqAPIKey)
                .CreateLogger();

            try
            {
                Log.Information("Start");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Error");
            }
            finally
            {
                Log.CloseAndFlush();
            }
            
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseSerilog();
                })
            .ConfigureServices(
            services => services.AddHostedService<CoreWarmupService>());

    }
}
