namespace LiveScore.Configurations
{
    public static class Configuration
    {
#if DEBUG
        public static string ApiEndPoint => "https://score247-api3.nexdev.net/dev/api";
        public static string SignalRHubEndPoint => "https://score247-api4.nexdev.net/dev/hubs";
        public static string AssetsEndPoint => "https://assets-dev.nexdev.net/test/";
        public static string Environment => "DEV";
#elif TEST
        public static string ApiEndPoint => "https://score247-api3.nexdev.net/test/api";
        public static string SignalRHubEndPoint => "https://score247-api4.nexdev.net/test/hubs";
        public static string AssetsEndPoint => "https://assets-dev.nexdev.net/test/";
        public static string Environment => "TEST";
#elif DEVRELEASE
        public static string ApiEndPoint => "https://score247-api3.nexdev.net/main/api";
        public static string SignalRHubEndPoint => "https://score247-api4.nexdev.net/main/hubs";
        public static string AssetsEndPoint => "https://assets-dev.nexdev.net/test/";
        public static string Environment => "DEV-RELEASE";
#else

        public static string ApiEndPoint => "https://score247-api3.nexdev.net/main/api";
        public static string SignalRHubEndPoint => "https://score247-api4.nexdev.net/main/hubs";
        public static string AssetsEndPoint => "https://assets-dev.nexdev.net/main/";
        public static string Environment => "MAIN";
#endif

        public static string SentryDsn => "https://a75e3e7b51ea4de8baa2c27b67bbede3@sentry.nexdev.net/34";
    }
}