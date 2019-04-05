﻿namespace Core.Services
{
    using System;
    using System.Diagnostics;
    using Common.Services;

    public class BaseService
    {
        private readonly ILoggingService loggingService;

        public BaseService(ILoggingService loggingService)
        {
            this.loggingService = loggingService;
        }

        protected virtual void HandleException(Exception ex)
        {
            loggingService.LogError(ex);
            Debug.WriteLine(ex.Message);
        }
    }
}
