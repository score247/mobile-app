using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using SharpRaven;
using SharpRaven.Data;
using Xamarin.Essentials;

namespace Common.Helpers.Logging
{
    public static class AnalyticsService
    {
        static Lazy<RavenClient> ravenClientHolder;

        public static void Init(string dsn) 
        {
            ravenClientHolder = new Lazy<RavenClient>(() => new RavenClient(dsn) { Release = AppInfo.VersionString });
        }

        static RavenClient RavenClient => ravenClientHolder.Value;

        public static void TrackEvent(string trackIdentifier, IDictionary<string, string> table = null) =>
            RavenClient.AddTrail(new Breadcrumb(trackIdentifier) { Data = table });

        public static void TrackEvent(string trackIdentifier, string key, string value)
        {
            if (string.IsNullOrWhiteSpace(key) && string.IsNullOrWhiteSpace(value))
                TrackEvent(trackIdentifier);
            else
                TrackEvent(trackIdentifier, new Dictionary<string, string> { { key, value } });
        }

        public static Task ReportAsync(Exception exception,
                                    [CallerMemberName] string callerMemberName = "",
                                    [CallerLineNumber] int lineNumber = 0,
                                    [CallerFilePath] string filePath = "")
        {
            return RavenClient.CaptureAsync(new SentryEvent(exception));
        }


        public static void Report(Exception exception)
        {
            RavenClient.Capture(CreateSentryEvent(exception));
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
