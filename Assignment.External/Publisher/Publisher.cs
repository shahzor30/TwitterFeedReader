namespace Assignment.External
{
    class Publisher
    {
        private readonly IStore store;
        public Publisher(IStore store)
        {
            this.store = store;
        }

        public void Publish(WebHookMessage item)
        {
            store.Queue.Enqueue(item);
            store.IncrementPublishedCount();
        }
    }
}