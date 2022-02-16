using System.Threading.Tasks;

namespace Assignment.External
{
    interface IOrchestrator
    {
        Task StartFeed();
        Task StartConsumer();
        Task StartAggregator();
        Task StartAverageReportMonitor();
        Task StartBasicReportMonitor();
    }
}