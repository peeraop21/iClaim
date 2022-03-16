using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
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
        private readonly IConfiguration configuration;


        public CoreWarmupService(ILogger<CoreWarmupService> logger, IWebHostEnvironment env, IConfiguration configuration)
        {
            _logger = logger;
            httpClient = new HttpClient();
            this.env = env;
            this.configuration = configuration;
        }

        public  Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting IHostedService...");
            if (env.IsDevelopment())
            {
                var baseUrl = configuration["BaseUrl:Local"];
                httpClient.GetAsync(baseUrl + "api/master/Warmup");
                //httpClient.GetAsync(baseUrl + "api/genpdf/WarmGenPDF");
            }
            else
            {
                var baseUrl = configuration["BaseUrl:Publish"];
                httpClient.GetAsync(baseUrl + "api/master/Warmup");
                //httpClient.GetAsync(baseUrl + "api/genpdf/WarmGenPDF");

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
