using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AppCenter;
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

    public class AppCenterLoggingService : ILoggingService
    {
        private readonly Dictionary<string, string> ClientInformation;
        private readonly IEssential essential;

        public AppCenterLoggingService(IEssential essential)
        {
            AppCenter.Start("ios=34adf4e9-18dd-4ef0-817f-48bce4ff7159;",
                typeof(Analytics), typeof(Crashes));

            this.essential = essential;

            ClientInformation = GenerateClientInfo();
        }

        private Dictionary<string, string> GenerateClientInfo() => new Dictionary<string, string>
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
            => Crashes.TrackError(exception, ClientInformation);

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
            => Analytics.TrackEvent(message, ClientInformation);

        public Task LogInfoAsync(string message)
            => Task.Run(() => LogInfo(message));

        public void TrackEvent(string trackIdentifier, string key, string value)
        {
            ClientInformation.Add(key, value);
            Analytics.TrackEvent(trackIdentifier, ClientInformation);
        }

        public void TrackEvent(string trackIdentifier, IDictionary<string, string> properties)
        {
            var clientInfo = GenerateClientInfo();

            properties?.ToList().ForEach(item =>
            {
                clientInfo.Add(item.Key, item.Value);
            });

            Analytics.TrackEvent(trackIdentifier, ClientInformation);
        }
    }
}