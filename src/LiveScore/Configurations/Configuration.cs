namespace LiveScore.Configurations
{
    public static class Configuration
    {
#if DEBUG
        public static string ApiEndPoint => "https://score247-api1.nexdev.net/main/api";

        public static string SignalRHubEndPoint => "https://score247-api2.nexdev.net/test/hubs/";

        public static string Environment => "DEV";
#elif TEST
        public static string ApiEndPoint => "https://score247-api1.nexdev.net/test/api/";
        chư vay
        public static string SignalRHubEndPoint => "https://score247-api2.nexdev.net/test/hubs/";

        public static string Environment => "TEST";
#elif AUTOTEST
        public static string ApiEndPoint => "https://score247-api1.nexdev.net/V4/api/";

        public static string SignalRHubEndPoint => "https://score247-api2.nexdev.net/V4/hubs/";

        public static string Environment => "AUTOTEST";
#elif DEVRELEASE
        public static string ApiEndPoint => "https://score247-api1.nexdev.net/main/api/";

        public static string SignalRHubEndPoint => "https://score247-api2.nexdev.net/main/hubs/";

        public static string Environment => "DEV-RELEASE";
#else

        public static string ApiEndPoint => "https://score247-api1.nexdev.net/main/api/";

        public static string SignalRHubEndPoint => "https://score247-api2.nexdev.net/main/hubs/";

        public static string Environment => "MAIN";
#endif

        public static string SentryDsn => "https://a75e3e7b51ea4de8baa2c27b67bbede3@sentry.nexdev.net/34";
    }
}