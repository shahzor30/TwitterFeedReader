using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Assignment.External
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                var service_provider = Bootstraper.Bootstrap();
                var logger = service_provider.GetRequiredService<ILogger>();

                logger.Log("Orchestrating...");
                var orchestrator = new BasicOrchestrator(service_provider, logger);

                logger.Log("starting consumer 1...");
                var consumer1_task = orchestrator.StartConsumer();

                logger.Log("starting consumer 2...");
                var consumer2_task = orchestrator.StartConsumer();

                logger.Log("starting stats aggregator...");
                var stats_task = orchestrator.StartAggregator();

                logger.Log("starting reporting...");
                var report_task1 = orchestrator.StartAverageReportMonitor();
                var report_task2 = orchestrator.StartBasicReportMonitor();

                logger.Log("starting feed...");
                try
                {
                    await orchestrator.StartFeed();
                }
                catch (Exception ex)
                {
                    logger.Log($"Error occurred in feed: {ex.Message}");
                }

                logger.Log("Program ended...");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred: {ex.Message}");
            }
        }
    }
}