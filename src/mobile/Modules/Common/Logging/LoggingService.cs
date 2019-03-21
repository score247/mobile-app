using System;
using System.Collections.Generic;
using Fanex.Logging;
using Fanex.Logging.Sentry.RavenClient;
using SharpRaven;

namespace Common.Logging
{
    public static class LoggingService
    {
        public static void Init() 
        { 
            if(LogManager == null)
            {
                LogManager
                 .SetDefaultLogCategory("Mobile")
                 .Use(new List<ILogEngine>
                 {
                     new SentryLogging(new SentryEngineOptions {
                        Dsn = new Dsn("https://3a1df1b72a8c43069712d4938730589a@sentry.io/1420554"),
                        Environment = "DEV"
                     })
                 });
            }          
        }

        public static LogManager LogManager { get; }

        public static void LogError(Exception exception) 
        {
            LogService.Error("Global exception", exception);
        }
    }
}
