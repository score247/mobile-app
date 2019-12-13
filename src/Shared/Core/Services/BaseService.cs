using System;
using LiveScore.Common.Services;
using Refit;

namespace LiveScore.Core.Services
{
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

                LoggingService.LogExceptionAsync(apiException, message);
            }
            else if (ex is AggregateException aggregateException)
            {
                foreach (var exception in aggregateException.InnerExceptions)
                {                    
                    LoggingService.LogExceptionAsync(exception, exception.Message);
                }
            }
            else
            {
                LoggingService.LogExceptionAsync(ex);
            }
        }
    }
}