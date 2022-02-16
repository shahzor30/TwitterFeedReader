using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Assignment.External
{
    class Bootstraper
    {
        public static IServiceProvider Bootstrap()
        {
            IConfiguration config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var services = new ServiceCollection();
            services.AddSingleton<IConfiguration>(config);
            services.AddSingleton<IStore, MemoryStore>();
            services.AddScoped<IConsumer, Consumer>();
            services.AddScoped<ILogger, ConsoleLogger>();
            services.AddSingleton<IStatsAggregator, StatsAggregator>();

            var environment = config.GetValue<string>("Environment") ?? "TEST";
            if (environment == "PROD")
            {
                services.AddSingleton<IConnector, TwitterConnector>();
            }
            else
            {
                services.AddSingleton<IConnector, ConsoleConnector>();
            }

            return services.BuildServiceProvider();
        }
    }
}