namespace Assignment.External
{
    interface IStatsAggregator
    {
        void Aggregate();
    }

    //class ConsumerHost
    //{
    //    private readonly IStore store;
    //    private readonly IConfiguration config;
    //    private readonly int consumers_count = 1;
    //    public ConsumerHost(IStore store, IConfiguration config)
    //    {
    //        this.store = store;
    //        this.config = config;
    //        consumers_count = config.GetValue<int?>("Consumers") ?? 1;
    //    }
    //    public void Run()
    //    {
    //        for (var i = 1; i <= consumers_count; i++)
    //        {                
    //            var task = Task.Run(async () =>
    //            {
    //                var consumer = new Consumer(store);
    //                while (true)
    //                {
    //                    consumer.Consume();
    //                    await Task.Delay(1000);
    //                }
    //            });
    //        }
    //    }
    //}
}