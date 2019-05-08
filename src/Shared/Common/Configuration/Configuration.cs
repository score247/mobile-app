namespace LiveScore.Common.Configuration
{
    public static class Configuration
    {
#if DEV
        public static string LocalEndPoint => "https://testing2.nexdev.net/V1/api/";
#elif TEST
        public static string LocalEndPoint => "https://testing2.nexdev.net/Test/api/";
#else
        public static string LocalEndPoint => "https://testing2.nexdev.net/Main/api/";
#endif
        public static string SentryDsn => "https://a75e3e7b51ea4de8baa2c27b67bbede3@sentry.nexdev.net/34";
    }
}