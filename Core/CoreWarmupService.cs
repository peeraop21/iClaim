using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Core
{
    public class CoreWarmupService : IHostedService, IDisposable
    {
        private readonly ILogger _logger;
        private readonly HttpClient httpClient;
        private readonly IWebHostEnvironment env;

        public CoreWarmupService(ILogger<CoreWarmupService> logger, IWebHostEnvironment env)
        {
            _logger = logger;
            httpClient = new HttpClient();
            this.env = env;
        }

        public  Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting IHostedService...");

            if (env.IsDevelopment())
            {
                httpClient.GetAsync("http://localhost:50598/api/master/Warmup");
                httpClient.GetAsync("http://localhost:50598/api/genpdf/WarmGenPDF");
            }
            else
            {
                httpClient.GetAsync("https://ts2digitalclaim.rvp.co.th/api/master/Warmup");
                httpClient.GetAsync("https://ts2digitalclaim.rvp.co.th/api/genpdf/WarmGenPDF");

            }


            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("StoppingIHostedService...");
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            httpClient?.Dispose();
        }
    }
}
