using API.Model;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace WorkerService
{
    public class Worker : BackgroundService
    {
        const int ThreadDelay = 5000;
        ILogger<Worker> log;
        HttpClient httpClient;
        string BaseURL = @"https://localhost:44300/GetBudGet";
        JsonSerializerOptions serializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        public Worker(ILogger<Worker> logger, IHttpClientFactory httpClient)
        {
            log = logger;
            this.httpClient = httpClient.CreateClient();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                log.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                try
                {
                    //Get data from API 
                    var budgetRs = await httpClient.GetFromJsonAsync<List<BudgetByMonth>>(BaseURL, serializerOptions, stoppingToken);
                    
                    foreach(var budget in budgetRs)
                    {
                        log.LogInformation($"Year: {budget.Year}, Month:{budget.Month}, TotalPrice:{budget.TotalPrice}");
                    }
                    // Serialize budgetRs to JSON string
                    var json = JsonSerializer.Serialize(budgetRs, serializerOptions);

                    
                    // Save JSON string to file
                    File.WriteAllText("budget.json", json);

                    log.LogInformation(new string('*', 30));
                }
                catch (HttpRequestException ex)
                {
                    log.LogInformation(ex.Message);
                    log.LogCritical($"{nameof(HttpRequestException)}: {ex.Message}");
                }

                await Task.Delay(ThreadDelay, stoppingToken);
            }
        }
    }
}
