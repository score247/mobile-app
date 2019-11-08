using LiveScore.Core;

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
        public string Enviroment => "Local";
        public bool Debug => true;

        //DataGenerator: http://ha.nexdev.net:7208/dev1
#elif TEST
        public string ApiEndPoint => "https://score247-api3.nexdev.net/test/api";
        public string SignalRHubEndPoint => "https://score247-api4.nexdev.net/test/hubs";
        public string AssetsEndPoint => "https://assets-dev.nexdev.net/test/";
        public string AppCenterSecret => "ios=34adf4e9-18dd-4ef0-817f-48bce4ff7159;";
        public string SentryDsn => "https://a75e3e7b51ea4de8baa2c27b67bbede3@sentry.nexdev.net/34";
        public string Enviroment => "Local";
        public bool Debug => true;


        //DataGenerator: http://ha.nexdev.net:7208/test
#elif LocalRelease
        public string ApiEndPoint => "https://score247-api3.nexdev.net/main/api";
        public string SignalRHubEndPoint => "https://score247-api4.nexdev.net/main/hubs";
        public string AssetsEndPoint => "https://assets-dev.nexdev.net/test/";
        public string AppCenterSecret => "ios=34adf4e9-18dd-4ef0-817f-48bce4ff7159;";
        public string SentryDsn => "https://a75e3e7b51ea4de8baa2c27b67bbede3@sentry.nexdev.net/34";
        public string Enviroment => "Local";
        public bool Debug => true;

#else

        public string ApiEndPoint => "https://score247-api3.nexdev.net/main/api";
        public string SignalRHubEndPoint => "https://score247-api4.nexdev.net/main/hubs";
        public string AssetsEndPoint => "https://assets-dev.nexdev.net/main/";
        public string AppCenterSecret => "ios=34adf4e9-18dd-4ef0-817f-48bce4ff7159;";
        public string SentryDsn => "https://a75e3e7b51ea4de8baa2c27b67bbede3@sentry.nexdev.net/34";
        public string Enviroment => "Testflight";
        public bool Debug => false;


#endif
    }
}