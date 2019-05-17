﻿namespace LiveScore.Common.Configuration
{
    public static class Configuration
    {
#if DEBUG
        public static string LocalEndPoint => "https://testing2.nexdev.net/V2/api/";

        public static string LocalHubEndPoint => "https://testing2.nexdev.net/V2/hubs/";
#elif TEST
        public static string LocalEndPoint => "https://testing2.nexdev.net/V2/api/";

         public static string LocalHubEndPoint => "https://testing2.nexdev.net/V2/hubs/";
#else
        public static string LocalEndPoint => "https://testing2.nexdev.net/Main/api/";

         public static string LocalHubEndPoint => "https://testing2.nexdev.net/Main/hubs/";
#endif
        public static string SentryDsn => "https://a75e3e7b51ea4de8baa2c27b67bbede3@sentry.nexdev.net/34";
    }
}