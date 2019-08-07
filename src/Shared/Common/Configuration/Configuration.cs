﻿namespace LiveScore.Common.Configuration
{
    public static class Configuration
    {
#if DEBUG

        //TODO change to dev
        public static string LocalEndPoint => "https://score247-api1.nexdev.net/test/api/";

        public static string LocalHubEndPoint => "https://score247-api2.nexdev.net/test/hubs/";
#elif TEST
        public static string LocalEndPoint => "https://score247-api1.nexdev.net/test/api/";

        public static string LocalHubEndPoint => "https://score247-api1.nexdev.net/test/hubs/";
#elif AUTOTEST
        public static string LocalEndPoint => "https://api.nexdev.net/V4/api/";

        public static string LocalHubEndPoint => "https://api.nexdev.net/V4/hubs/";
#else
        public static string LocalEndPoint => "https://api.nexdev.net/Main/api/";

        public static string LocalHubEndPoint => "https://api.nexdev.net/Main/hubs/";
#endif
        public static string SentryDsn => "https://a75e3e7b51ea4de8baa2c27b67bbede3@sentry.nexdev.net/34";
    }
}