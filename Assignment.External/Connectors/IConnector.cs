using System.Collections.Generic;

namespace Assignment.External
{
    public interface IConnector
    {
        IAsyncEnumerable<WebHookMessage> GetMessag();
    }
}
