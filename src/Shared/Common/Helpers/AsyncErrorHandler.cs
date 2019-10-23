using System;
using System.Diagnostics;
using LiveScore.Common.Services;

namespace LiveScore.Common.Helpers
{
    public static class AsyncErrorHandler
    {
        private readonly static LoggingService loggingService = new LoggingService();

        public static void HandleException(Exception exception)
        {
            loggingService?.LogExceptionAsync(exception);
            Debug.WriteLine(exception);
        }
    }
}