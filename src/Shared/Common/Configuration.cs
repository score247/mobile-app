﻿namespace LiveScore.Common
{
    public interface IConfiguration
    {
        string ApiEndPoint { get; }

        string SignalRHubEndPoint { get; }

        string AssetsEndPoint { get; }

        string AppCenterSecret { get; }

        string SentryDsn { get; }

        string Enviroment { get; }

        bool Debug { get; }

        string EncryptKey { get; }
    }
}