using LiveScore.Core;

namespace LiveScore.Configurations
{
    public class Configuration : IConfiguration
    {
#if DEBUG
        public string ApiEndPoint => "https://score247-api3.nexdev.net/main/api";
        public string SignalRHubEndPoint => "https://score247-api4.nexdev.net/main/hubs";
        public string AssetsEndPoint => "https://assets-dev.nexdev.net/main/";
        public string Environment => "MAIN";
#elif TEST
        public string ApiEndPoint => "https://score247-api3.nexdev.net/test/api";
        public string SignalRHubEndPoint => "https://score247-api4.nexdev.net/test/hubs";
        public string AssetsEndPoint => "https://assets-dev.nexdev.net/test/";
        public string Environment => "TEST";
#elif DEVRELEASE
        public string ApiEndPoint => "https://score247-api3.nexdev.net/main/api";
        public string SignalRHubEndPoint => "https://score247-api4.nexdev.net/main/hubs";
        public string AssetsEndPoint => "https://assets-dev.nexdev.net/test/";
        public string Environment => "DEV-RELEASE";
#else

        public string ApiEndPoint => "https://score247-api3.nexdev.net/main/api";
        public string SignalRHubEndPoint => "https://score247-api4.nexdev.net/main/hubs";
        public string AssetsEndPoint => "https://assets-dev.nexdev.net/main/";
        public string Environment => "MAIN";
#endif

        public string SentryDsn => "https://a75e3e7b51ea4de8baa2c27b67bbede3@sentry.nexdev.net/34";
    }
}