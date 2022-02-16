using System.Collections.Concurrent;
using System.Threading;

namespace Assignment.External
{
    class MemoryStore : IStore
    {
        private long published_count = 0;
        private long processed_count = 0;

        public ConcurrentQueue<WebHookMessage> Queue { get; set; } = new ConcurrentQueue<WebHookMessage>();
        public ConcurrentBag<long> Statistics { get; set; } = new ConcurrentBag<long>();
        public long GetPublishedCount()
        {
            return published_count;
        }
        public void IncrementPublishedCount()
        {
            Interlocked.Increment(ref published_count);
        }

        public long GetProcessedCount()
        {
            return processed_count;
        }
        public void IncrementProcessedCount()
        {
            Interlocked.Increment(ref processed_count);
        }

        public void AddStatistic(long stat)
        {
            Statistics.Add(stat);
        }

    }
}