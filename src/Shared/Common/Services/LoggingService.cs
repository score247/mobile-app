namespace LiveScore.Common.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using SharpRaven;
    using SharpRaven.Data;

    public interface ILoggingService
    {
        Task LogErrorAsync(Exception exception);

        void LogError(Exception exception);

        void TrackEvent(string trackIdentifier, string key, string value);

        void Init(string Dsn, IRavenClient ravenClient = null);
    }

    public class LoggingService : ILoggingService
    {
        private readonly IEssentialsService deviceInfo;

        private IRavenClient ravenClient;

        public LoggingService(IEssentialsService deviceInfo)
        {
            this.deviceInfo = deviceInfo;
        }

        public void Init(string Dsn, IRavenClient ravenClient = null)
        {
            this.ravenClient = ravenClient ?? new RavenClient(Dsn) { Release = deviceInfo.AppVersion };
        }

        public void LogError(Exception exception) => ravenClient?.Capture(CreateSentryEvent(exception));

        public Task LogErrorAsync(Exception exception) => ravenClient?.CaptureAsync(CreateSentryEvent(exception));

        private SentryEvent CreateSentryEvent(Exception exception)
        {
            var evt = new SentryEvent(exception);

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
