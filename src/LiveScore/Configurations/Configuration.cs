using LiveScore.Common;

namespace LiveScore.Configurations
{
    public class Configuration : IConfiguration
    {
#if DEBUG
        public string ApiEndPoint => "https://score247-api3.nexdev.net/dev/api";
        public string SignalRHubEndPoint => "https://score247-api4.nexdev.net/dev/hubs";
        public string AssetsEndPoint => "https://assets-dev.nexdev.net/test/";
        public string AppCenterSecret => "ios=b08e2753-b596-44c9-bed9-701d8dd8be8c;";
        public string SentryDsn => "https://a75e3e7b51ea4de8baa2c27b67bbede3@sentry.nexdev.net/34";
        public string Environment => "Local";
        public bool Debug => true;
        public string EncryptKey => "530c4b6a5f7451912b270aa140712e52";

        //DataGenerator: http://ha.nexdev.net:7208/dev1
#elif TEST
        public string ApiEndPoint => "https://score247-api3.nexdev.net/test/api";
        public string SignalRHubEndPoint => "https://score247-api4.nexdev.net/test/hubs";
        public string AssetsEndPoint => "https://assets-dev.nexdev.net/test/";
        public string AppCenterSecret => "ios=b08e2753-b596-44c9-bed9-701d8dd8be8c;";
        public string SentryDsn => "https://a75e3e7b51ea4de8baa2c27b67bbede3@sentry.nexdev.net/34";
        public string Environment => "Local";
        public bool Debug => true;
        public string EncryptKey => "e6e42fa2cd8db55e10c2b7b4833e0f8e";

        //DataGenerator: http://ha.nexdev.net:7208/test
#elif LocalRelease
        public string ApiEndPoint => "https://score247-api3.nexdev.net/main/api";
        public string SignalRHubEndPoint => "https://score247-api4.nexdev.net/main/hubs";
        public string AssetsEndPoint => "https://assets-dev.nexdev.net/test/";
        public string AppCenterSecret => "ios=b08e2753-b596-44c9-bed9-701d8dd8be8c;";
        public string SentryDsn => "https://a75e3e7b51ea4de8baa2c27b67bbede3@sentry.nexdev.net/34";
        public string Environment => "Local";
        public bool Debug => true;
        public string EncryptKey => "350cd642f6764a154a9e4f03eb3121b6";

#else

        public string ApiEndPoint => "https://score247-api3.nexdev.net/main/api";
        public string SignalRHubEndPoint => "https://score247-api4.nexdev.net/main/hubs";
        public string AssetsEndPoint => "https://assets-dev.nexdev.net/main/";
        public string AppCenterSecret => "ios=34adf4e9-18dd-4ef0-817f-48bce4ff7159;";
        public string SentryDsn => "https://a75e3e7b51ea4de8baa2c27b67bbede3@sentry.nexdev.net/34";
        public string Environment => "Testflight";
        public bool Debug => false;
        public string EncryptKey => "350cd642f6764a154a9e4f03eb3121b6";

#endif
    }
}