namespace Assignment.External
{
    class StatsAggregator : IStatsAggregator
    {
        private readonly IStore store;
        private long history_processed_count = 0;
        public StatsAggregator(IStore store)
        {
            this.store = store;
        }
        public void Aggregate()
        {
            var processed_count = store.GetProcessedCount();
            var processed = processed_count - history_processed_count;
            store.AddStatistic(processed);

            history_processed_count = processed_count;
        }
    }    
}