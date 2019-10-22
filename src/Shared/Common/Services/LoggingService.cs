using System;
using System.Collections.Generic;
using System.Linq;
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

        Task TrackEventAsync(string trackIdentifier, string message);

        void TrackEvent(string trackIdentifier, IDictionary<string, string> properties);

        Task TrackEventAsync(string trackIdentifier, IDictionary<string, string> properties);
    }

    public class LoggingService : ILoggingService
    {
        private readonly Dictionary<string, string> ClientInformation;
        private readonly Action<Exception, IDictionary<string, string>> trackError;
        private readonly Action<string, IDictionary<string, string>> trackEvent;

        public LoggingService(
            IEssential essential,
            Action<Exception, IDictionary<string, string>> trackError = null,
            Action<string, IDictionary<string, string>> trackEvent = null)
        {
            this.trackError = trackError ?? Crashes.TrackError;
            this.trackEvent = trackEvent ?? Analytics.TrackEvent;

            ClientInformation = GenerateClientInfo(essential);
        }

        public void LogException(Exception exception)
            => trackError(exception, ClientInformation);

        public void LogException(Exception exception, string message)
        {
            var properties = new Dictionary<string, string>
            {
                ["Message"] = message
            };

            LogException(exception, properties);
        }

        public void LogException(Exception exception, IDictionary<string, string> properties)
        {
            AddClientInformation(properties);

            trackError(exception, properties);
        }

        public Task LogExceptionAsync(Exception exception)
            => Task.Run(() => LogException(exception));

        public Task LogExceptionAsync(Exception exception, string message)
            => Task.Run(() => LogException(exception, message));

        public Task LogExceptionAsync(Exception exception, IDictionary<string, string> properties)
            => Task.Run(() => LogException(exception, properties));

        public void TrackEvent(string trackIdentifier, string message)
        {
            var properties = new Dictionary<string, string>
            {
                ["Message"] = message
            };

            TrackEvent(trackIdentifier, properties);
        }

        public Task TrackEventAsync(string trackIdentifier, string message)
            => Task.Run(() => TrackEvent(trackIdentifier, message));

        public void TrackEvent(string trackIdentifier, IDictionary<string, string> properties)
        {
            AddClientInformation(properties);

            trackEvent(trackIdentifier, properties);
        }

        public Task TrackEventAsync(string trackIdentifier, IDictionary<string, string> properties)
             => Task.Run(() => TrackEvent(trackIdentifier, properties));

        private static Dictionary<string, string> GenerateClientInfo(IEssential essential) => new Dictionary<string, string>
        {
            ["AppName"] = essential.AppName,
            ["AppVersion"] = essential.AppVersion,
            ["Device.Model"] = essential.Model,
            ["Device.Name"] = essential.Name,
            ["OperatingSystem.Name"] = essential.OperatingSystemName,
            ["OperatingSystem.Version"] = essential.OperatingSystemVersion,
        };

        private void AddClientInformation(IDictionary<string, string> properties)
        {
            ClientInformation?.ToList().ForEach(item =>
            {
                properties.Add(item.Key, item.Value);
            });
        }
    }
}