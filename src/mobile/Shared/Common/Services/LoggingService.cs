namespace Common.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using SharpRaven;
    using SharpRaven.Data;
    using Xamarin.Essentials;

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
        private readonly IDeviceInfoService deviceInfo;
        private readonly ISettingsService settingsService;

        private static RavenClient RavenClient;

        public SentryLogger(IDeviceInfoService deviceInfo, ISettingsService settingsService)
        {
            this.deviceInfo = deviceInfo;
            this.settingsService = settingsService;

            RavenClient = new RavenClient(settingsService.Dsn);
        }

        public SentryEvent CreateSentryEvent(Exception exception)
        {
            RavenClient.Release = AppInfo.VersionString;
            var evt = new SentryEvent(exception);

            evt.Contexts.Device.Model = deviceInfo.Model;
            evt.Contexts.Device.Name = deviceInfo.Name;
            evt.Contexts.OperatingSystem.Name = deviceInfo.OperatingSystemName;
            evt.Contexts.OperatingSystem.Version = deviceInfo.OperatingSystemVersion;

            return evt;
        }

        public void LogError(Exception exception) => RavenClient.Capture(CreateSentryEvent(exception));

        public Task LogErrorAsync(Exception exception) => RavenClient.CaptureAsync(CreateSentryEvent(exception));

        public void TrackEvent(string trackIdentifier, IDictionary<string, string> table = null)
            => RavenClient.AddTrail(
                new Breadcrumb(trackIdentifier)
                {
                    Data = table
                });

        public void TrackEvent(string trackIdentifier, string key, string value)
            => TrackEvent(trackIdentifier, new Dictionary<string, string> { { key, value } });
    }
}
