namespace Assignment.External
{
    class Consumer : IConsumer
    {
        private readonly IStore store;
        public Consumer(IStore store)
        {
            this.store = store;
        }

        public void Consume()
        {
            while (store.Queue.TryDequeue(out WebHookMessage value))
            {                
                store.IncrementProcessedCount();
            }
        }
    }
}