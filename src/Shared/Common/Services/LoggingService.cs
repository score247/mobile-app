using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;

namespace LiveScore.Common.Services
{
    public interface ILoggingService
    {
        Task LogExceptionAsync(Exception exception);

        Task LogExceptionAsync(Exception exception, string message);

        Task LogExceptionAsync(Exception exception, IDictionary<string, string> properties);

        void LogException(Exception exception);

        void LogException(Exception exception, string message);

        void LogException(Exception exception, IDictionary<string, string> properties);

        void TrackEvent(string trackIdentifier, string message);

        void TrackEvent(string trackIdentifier, IDictionary<string, string> properties);

        Task TrackEventAsync(string trackIdentifier, IDictionary<string, string> properties);

        Task TrackEventAsync(string trackIdentifier, string message);
    }

    public class LoggingService : ILoggingService
    {
        private readonly Action<Exception, IDictionary<string, string>> trackError;
        private readonly Action<string, IDictionary<string, string>> trackEvent;

        public LoggingService(

            Action<Exception, IDictionary<string, string>> trackError = null,
            Action<string, IDictionary<string, string>> trackEvent = null)
        {
            this.trackError = trackError ?? Crashes.TrackError;
            this.trackEvent = trackEvent ?? Analytics.TrackEvent;
        }

        public void LogException(Exception exception)
            => trackError(exception, null);

        public void LogException(Exception exception, string message)
            => LogException(exception, new Dictionary<string, string>
            {
                ["message"] = message
            });

        public void LogException(Exception exception, IDictionary<string, string> properties)
            => trackError(exception, properties);

        public Task LogExceptionAsync(Exception exception)
            => Task.Run(() => LogException(exception));

        public Task LogExceptionAsync(Exception exception, string message)
            => Task.Run(() => LogException(exception, message));

        public Task LogExceptionAsync(Exception exception, IDictionary<string, string> properties)
            => Task.Run(() => LogException(exception, properties));

        public void TrackEvent(string trackIdentifier, IDictionary<string, string> properties)
           => trackEvent(trackIdentifier, properties);

        public void TrackEvent(string trackIdentifier, string message)
            => TrackEvent(trackIdentifier, new Dictionary<string, string>
            {
                ["message"] = message
            });

        public Task TrackEventAsync(string trackIdentifier, string message)
            => Task.Run(() => TrackEvent(trackIdentifier, message));

        public Task TrackEventAsync(string trackIdentifier, IDictionary<string, string> properties)
             => Task.Run(() => TrackEvent(trackIdentifier, properties));
    }
}