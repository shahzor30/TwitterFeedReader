using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace Assignment.External
{
    public class ConsoleConnector : IConnector
    {
        private readonly ConsoleConfig? _settings;
        public ConsoleConnector(IConfiguration config)
        {
            _settings = config.GetRequiredSection("Connectors:Console").Get<ConsoleConfig>();
        }

        public async IAsyncEnumerable<WebHookMessage> GetMessag()
        {            
            DateTime end = DateTime.Now.AddSeconds(_settings?.Duration ?? 60);
            var counter = 1;

            while (DateTime.Now < end)
            {
                yield return new WebHookMessage { Id = Guid.NewGuid().ToString(), Message = $"Test Message {counter++}" };                
                await System.Threading.Tasks.Task.Delay(1);
            }
        }

        private class ConsoleConfig
        {
            public int Duration { get; set; }
        }
    }    
}
