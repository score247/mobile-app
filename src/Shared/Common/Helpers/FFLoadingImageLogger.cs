using System;
using FFImageLoading.Helpers;
using LiveScore.Common.Services;

namespace LiveScore.Common.Helpers
{
    public class FFLoadingImageLogger : IMiniLogger
    {
        private readonly ILoggingService loggingService;

        public FFLoadingImageLogger(ILoggingService loggingService)
        {
            this.loggingService = loggingService;
        }

        public void Debug(string message)
            => System.Diagnostics.Debug.WriteLine(message);

        public void Error(string errorMessage)
            => loggingService.LogException(new Exception(errorMessage));

        public void Error(string errorMessage, Exception ex)
            => loggingService.LogException(errorMessage, ex);
    }
}