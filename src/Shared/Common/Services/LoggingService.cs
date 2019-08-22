using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SharpRaven;
using SharpRaven.Data;

namespace LiveScore.Common.Services
{
    public interface ILoggingService
    {
        Task LogErrorAsync(Exception exception);

        Task LogErrorAsync(string message, Exception exception);

        void LogError(Exception exception);

        void LogError(string message, Exception exception);

        void TrackEvent(string trackIdentifier, string key, string value);

        void Init(string Dsn, IRavenClient ravenClient = null);
    }

    public class LoggingService : ILoggingService
    {
        private readonly IEssential deviceInfo;

        private IRavenClient ravenClient;

        public LoggingService(IEssential deviceInfo)
        {
            this.deviceInfo = deviceInfo;
        }

        public void Init(string Dsn, IRavenClient ravenClient = null)
        {
            this.ravenClient = ravenClient ?? new RavenClient(Dsn) { Release = deviceInfo.AppVersion };
        }

        public void LogError(Exception exception) => ravenClient?.Capture(CreateSentryEvent(string.Empty, exception));

        public void LogError(string message, Exception exception) => ravenClient?.Capture(CreateSentryEvent(message, exception));

        public Task LogErrorAsync(Exception exception) => ravenClient?.CaptureAsync(CreateSentryEvent(string.Empty, exception));

        public Task LogErrorAsync(string message, Exception exception) => ravenClient?.CaptureAsync(CreateSentryEvent(message, exception));

        private SentryEvent CreateSentryEvent(string message, Exception exception)
        {
            var evt = new SentryEvent(exception);

            if (!string.IsNullOrWhiteSpace(message))
            {
                evt.Message = new SentryMessage(message);
            }            

            evt.Contexts.Device.Model = deviceInfo.Model;
            evt.Contexts.Device.Name = deviceInfo.Name;
            evt.Contexts.OperatingSystem.Name = deviceInfo.OperatingSystemName;
            evt.Contexts.OperatingSystem.Version = deviceInfo.OperatingSystemVersion;

            return evt;
        }

        private void TrackEvent(string trackIdentifier, IDictionary<string, string> table)
            => ravenClient?.AddTrail(
                new Breadcrumb(trackIdentifier)
                {
                    Data = table
                });

        public void TrackEvent(string trackIdentifier, string key, string value)
            => TrackEvent(trackIdentifier, new Dictionary<string, string> { { key, value } });
    }
}