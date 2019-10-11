namespace LiveScore.Core.Services
{
    using System;
    using LiveScore.Common.Services;
    using Refit;

    public class BaseService
    {
        protected readonly ILoggingService LoggingService;

        public BaseService(ILoggingService loggingService)
        {
            LoggingService = loggingService;
        }

        protected virtual void HandleException(Exception ex)
        {
            if (ex is ApiException apiException)
            {
                var message = $"Response: {apiException?.Content} \r\nRequest URL: {apiException?.RequestMessage?.RequestUri}";

                LoggingService.LogExceptionAsync(apiException);
            }
            else
            {
                LoggingService.LogExceptionAsync(ex);
            }
        }
    }
}