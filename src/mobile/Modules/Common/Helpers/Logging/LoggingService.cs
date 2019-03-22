namespace Common.Helpers.Logging
{
    using System;

    public static class LoggingService
    {

        public static void Init(string category, string environment, string dsn)
        {
            AnalyticsService.Init(dsn);
        }

        public static void LogError(string message, Exception exception)
        {
            AnalyticsService.Report(exception);
        }

        public static void TrackEvent(string identifier)
        {
            AnalyticsService.TrackEvent(identifier);
        }
    }
}
