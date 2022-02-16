using System.Collections.Concurrent;

namespace Assignment.External
{
    interface IStore
    {
        ConcurrentQueue<WebHookMessage> Queue { get; set; }
        ConcurrentBag<long> Statistics { get; set; }
        long GetPublishedCount();
        void IncrementPublishedCount();

        long GetProcessedCount();
        void IncrementProcessedCount();

        void AddStatistic(long stat);
    }
}