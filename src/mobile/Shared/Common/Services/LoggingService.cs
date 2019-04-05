namespace Common.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using SharpRaven;
    using SharpRaven.Data;

    public interface ILoggingService
    {
        void TrackEvent(string trackIdentifier, IDictionary<string, string> table = null);

        void TrackEvent(string trackIdentifier, string key, string value);

        Task LogErrorAsync(Exception exception);

        void LogError(Exception exception);

        SentryEvent CreateSentryEvent(Exception exception);
    }

    public class SentryLogger : ILoggingService
    {
        private const string Dsn = "https://a75e3e7b51ea4de8baa2c27b67bbede3@sentry.nexdev.net/34";
        private readonly IEssentialsService deviceInfo;

        private readonly RavenClient ravenClient;

        public SentryLogger(IEssentialsService deviceInfo)
        {
            this.deviceInfo = deviceInfo;

            ravenClient = new RavenClient(Dsn) { Release = deviceInfo.AppVersion };
        }

        public SentryEvent CreateSentryEvent(Exception exception)
        {
            var evt = new SentryEvent(exception);

            evt.Contexts.Device.Model = deviceInfo.Model;
            evt.Contexts.Device.Name = deviceInfo.Name;
            evt.Contexts.OperatingSystem.Name = deviceInfo.OperatingSystemName;
            evt.Contexts.OperatingSystem.Version = deviceInfo.OperatingSystemVersion;

            return evt;
        }

        public void LogError(Exception exception) => ravenClient.Capture(CreateSentryEvent(exception));

        public Task LogErrorAsync(Exception exception) => ravenClient.CaptureAsync(CreateSentryEvent(exception));

        public void TrackEvent(string trackIdentifier, IDictionary<string, string> table = null)
            => ravenClient.AddTrail(
                new Breadcrumb(trackIdentifier)
                {
                    Data = table
                });

        public void TrackEvent(string trackIdentifier, string key, string value)
            => TrackEvent(trackIdentifier, new Dictionary<string, string> { { key, value } });
    }
}
