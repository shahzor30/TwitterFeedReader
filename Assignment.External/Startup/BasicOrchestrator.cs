using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment.External
{
    class BasicOrchestrator : IOrchestrator
    {
        private readonly IServiceProvider service_provider;
        private readonly ILogger logger;
        public BasicOrchestrator(IServiceProvider service_provider, ILogger logger)
        {
            this.service_provider = service_provider;
            this.logger = logger;
        }
        public Task StartFeed()
        {
            return Task.Run(async () =>
            {
                var connector = service_provider.GetRequiredService<IConnector>();
                var store = service_provider.GetRequiredService<IStore>();
                var publisher = new Publisher(store);
                await foreach (var msg in connector.GetMessag())
                {
                    publisher.Publish(msg);
                }
            });
        }

        public Task StartConsumer()
        {
            return Task.Run(async () =>
            {
                var consumer = service_provider.GetRequiredService<IConsumer>();
                while (true)
                {
                    consumer.Consume();
                    await Task.Delay(1000);
                }
            });
        }

        public Task StartAggregator()
        {
            return Task.Run(async () =>
            {
                var stats_aggregator = service_provider.GetRequiredService<IStatsAggregator>();
                while (true)
                {
                    await Task.Delay(60000);            // runs every 1 min
                    stats_aggregator.Aggregate();
                }
            });
        }

        public Task StartAverageReportMonitor()
        {
            return Task.Run(async () =>
            {
                var store = service_provider.GetRequiredService<IStore>();
                while (true)
                {
                    await Task.Delay(90000);            // runs every 1.5 min
                    if (store.Statistics.Any())
                    {
                        var avg = (int)store.Statistics.Average(x => x);
                        logger.Log($"Average tweets per minute: {avg}");
                    }
                }
            });
        }

        public Task StartBasicReportMonitor()
        {
            return Task.Run(async () =>
            {
                var store = service_provider.GetRequiredService<IStore>();
                while (true)
                {
                    await Task.Delay(1000);             // runs every 1 second
                    logger.Log($"Total tweets received: {store.GetPublishedCount()} Total tweets processed: {store.GetProcessedCount()}");
                }
            });
        }
    }
}