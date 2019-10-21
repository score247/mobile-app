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

        Task LogExceptionAsync(string message, Exception exception);

        void LogException(Exception exception);

        void LogException(string message, Exception exception);

        Task LogInfoAsync(string message);

        void LogInfo(string message);

        void TrackEvent(string trackIdentifier, IDictionary<string, string> properties);
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

        private static Dictionary<string, string> GenerateClientInfo(IEssential essential) => new Dictionary<string, string>
        {
            ["AppName"] = essential.AppName,
            ["AppVersion"] = essential.AppVersion,
            ["Device.Model"] = essential.Model,
            ["Device.Name"] = essential.Name,
            ["OperatingSystem.Name"] = essential.OperatingSystemName,
            ["OperatingSystem.Version"] = essential.OperatingSystemVersion,
            ["Message"] = string.Empty
        };

        public void LogException(Exception exception)
            => trackError(exception, ClientInformation);

        public void LogException(string message, Exception exception)
        {
            ClientInformation["Message"] = message;

            LogException(exception);
        }

        public Task LogExceptionAsync(Exception exception)
            => Task.Run(() => LogException(exception));

        public Task LogExceptionAsync(string message, Exception exception)
            => Task.Run(() => LogException(message, exception));

        public void LogInfo(string message)
            => trackEvent(message, ClientInformation);

        public Task LogInfoAsync(string message)
            => Task.Run(() => LogInfo(message));

        public void TrackEvent(string trackIdentifier, string key, string value)
        {
            ClientInformation.Add(key, value);
            trackEvent(trackIdentifier, ClientInformation);
        }

        public void TrackEvent(string trackIdentifier, IDictionary<string, string> properties)
        {
            var clientInfo = ClientInformation;

            properties?.ToList().ForEach(item =>
            {
                clientInfo.Add(item.Key, item.Value);
            });

            trackEvent(trackIdentifier, clientInfo);
        }
    }
}