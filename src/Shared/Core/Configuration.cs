using System;
using System.Collections.Generic;
using System.Text;

namespace LiveScore.Core
{
    public interface IConfiguration
    {
        string ApiEndPoint { get; }

        string SignalRHubEndPoint { get; }

        string AssetsEndPoint { get; }

        string Environment { get; }

        string SentryDsn { get; }
    }
}
