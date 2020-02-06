using System;
using LiveScore.Common.Services;
using Refit;

namespace LiveScore.Core.Services
{
    public class BaseService
    {
        protected readonly ILoggingService LoggingService;

        protected BaseService(ILoggingService loggingService)
        {
            LoggingService = loggingService;
        }

        protected virtual void HandleException(Exception ex)
        {
            switch (ex)
            {
                case ApiException apiException:
                    {
                        var message = $"Response: {apiException?.Content} \r\nRequest URL: {apiException?.RequestMessage?.RequestUri}";

                        LoggingService.LogExceptionAsync(apiException, message);
                        break;
                    }
                case AggregateException aggregateException:
                    {
                        foreach (var exception in aggregateException.InnerExceptions)
                        {
                            LoggingService.LogExceptionAsync(exception, exception.Message);
                        }

                        break;
                    }
                default:
                    LoggingService.LogExceptionAsync(ex);
                    break;
            }
        }
    }
}