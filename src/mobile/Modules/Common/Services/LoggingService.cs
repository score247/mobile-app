namespace Common.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using SharpRaven;
    using SharpRaven.Data;
    using Xamarin.Essentials;

    public static class LoggingService
    {
        static Lazy<RavenClient> ravenClientHolder;
        static RavenClient RavenClient => ravenClientHolder.Value;

        public static void Init(string category, string environment, string dsn)
        {
            ravenClientHolder = new Lazy<RavenClient>(() => new RavenClient(dsn) { Release = AppInfo.VersionString });
        }

        public static Task LogErrorAsync(Exception exception) => RavenClient.CaptureAsync(CreateSentryEvent(exception));

        public static void LogError(Exception exception)
        {
            RavenClient.Capture(CreateSentryEvent(exception));
        }

        public static void TrackEvent(string trackIdentifier, IDictionary<string, string> table = null) =>
            RavenClient.AddTrail(new Breadcrumb(trackIdentifier) { Data = table });

        public static void TrackEvent(string trackIdentifier, string key, string value)
        {
            if (string.IsNullOrWhiteSpace(key) && string.IsNullOrWhiteSpace(value))
            {
                TrackEvent(trackIdentifier);
            }
            else
            {
                TrackEvent(trackIdentifier, new Dictionary<string, string> { { key, value } });
            }
        }

        private static SentryEvent CreateSentryEvent(Exception exception)
        {
            var evt = new SentryEvent(exception);

            evt.Contexts.Device.Model = DeviceInfo.Model;
            evt.Contexts.Device.Name = DeviceInfo.Name;
            evt.Contexts.OperatingSystem.Name = DeviceInfo.Platform.ToString();
            evt.Contexts.OperatingSystem.Version = DeviceInfo.VersionString;

            return evt;
        }
    }
}
