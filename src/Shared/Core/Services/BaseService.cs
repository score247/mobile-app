namespace LiveScore.Core.Services
{
    using System;
    using System.Diagnostics;
    using LiveScore.Common.Services;
    using Refit;

    public class BaseService
    {
        protected readonly ILoggingService loggingService;

        public BaseService(ILoggingService loggingService)
        {
            this.loggingService = loggingService;
        }

        protected virtual void HandleException(Exception ex)
        {
            var apiException = ex as ApiException;

            if (apiException != null)
            {               
                var message = $"Response: {apiException?.Content} \r\nRequest URL: {apiException?.RequestMessage?.RequestUri}";

                loggingService.LogError(message, apiException);
            }
            else
            {
                loggingService.LogError(ex);
            }           
        }
    }
}