using System;
using System.Collections.Generic;
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

        protected virtual void HandleException(Exception ex, IDictionary<string, string> properties = null)
        {
            

            switch (ex)
            {
                case ApiException apiException:
                    {
                        LoggingService.LogExceptionAsync(apiException, properties ?? new Dictionary<string, string>());
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
                    LoggingService.LogExceptionAsync(ex, properties ?? new Dictionary<string,string>());
                    break;
            }
        }
    }
}