using System;
using System.Collections.Generic;
using Fanex.Logging;
using Fanex.Logging.Sentry.RavenClient;
using SharpRaven;

namespace Common.Logging
{
    public static class LoggingService
    {

        public static void Init(string category, string environment, string dsn) 
        { 
            if(LogManager == null)
            {
                LogManager
                 .SetDefaultLogCategory(category)
                 .Use(new List<ILogEngine>
                 {
                     new SentryLogging(new SentryEngineOptions {
                        Dsn = new Dsn(dsn),
                        Environment = environment
                     })
                 });
            }          
        }

        public static LogManager LogManager { get; }

        public static void LogError(string message, Exception exception) 
        {
            LogService.Error(message, exception);
        }
    }
}
