using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Sentry;
using Sentry.Protocol;
using Xamarin.Essentials;

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
        private readonly string KeyEmpty = string.Empty;
        private readonly Action<Exception, IDictionary<string, string>> trackError;
        private readonly Action<string, IDictionary<string, string>> trackEvent;
        private readonly INetworkConnection networkConnection;

        public LoggingService(
            INetworkConnection networkConnection,
            Action<Exception, IDictionary<string, string>> trackError = null,
            Action<string, IDictionary<string, string>> trackEvent = null,
            Func<bool> isSentryEnable = null)
        {
            this.networkConnection = networkConnection;

            if (isSentryEnable?.Invoke() == true || SentrySdk.IsEnabled)
            {
                this.trackError = trackError ?? ((ex, properties) =>
                {
                    Crashes.TrackError(ex, properties);
                    SentryLogger.LogException(ex, properties);
                });

                this.trackEvent = trackEvent ?? ((message, properties) =>
                {
                    Analytics.TrackEvent(message, properties);
                    SentryLogger.LogInfo(message, properties);
                });
            }
            else
            {
                this.trackError = trackError ?? Crashes.TrackError;
                this.trackEvent = trackEvent ?? Analytics.TrackEvent;
            }
        }

        public void LogException(Exception exception)
            => LogException(exception, string.Empty);

        public void LogException(Exception exception, string message)
            => LogException(exception, new Dictionary<string, string> { [KeyEmpty] = message });

        public void LogException(Exception exception, IDictionary<string, string> properties)
        {
            if (networkConnection.IsSuccessfulConnection())
            {
                trackError(exception, properties);
            }
        }

        public Task LogExceptionAsync(Exception exception)
            => Task.Run(() => LogException(exception));

        public Task LogExceptionAsync(Exception exception, string message)
            => Task.Run(() => LogException(exception, message));

        public Task LogExceptionAsync(Exception exception, IDictionary<string, string> properties)
            => Task.Run(() => LogException(exception, properties));

        public void TrackEvent(string trackIdentifier, IDictionary<string, string> properties)
        {
            if (networkConnection.IsSuccessfulConnection())
            {
                trackEvent(trackIdentifier, properties);
            }
        }

        public void TrackEvent(string trackIdentifier, string message)
            => TrackEvent(trackIdentifier, new Dictionary<string, string> { [KeyEmpty] = message });

        public Task TrackEventAsync(string trackIdentifier, string message)
            => Task.Run(() => TrackEvent(trackIdentifier, message));

        public Task TrackEventAsync(string trackIdentifier, IDictionary<string, string> properties)
             => Task.Run(() => TrackEvent(trackIdentifier, properties));
    }

    public static class SentryLogger
    {
        public static Task LogExceptionAsync(Exception exception)
            => CaptureAsync(CreateSentryEvent(exception));

        public static Task LogExceptionAsync(string message, Exception exception)
            => CaptureAsync(CreateSentryEvent(exception, message));

        public static void LogException(Exception exception, IDictionary<string, string> properties)
             => Capture(CreateSentryEvent(exception, properties));

        public static void LogException(Exception exception)
            => Capture(CreateSentryEvent(exception));

        public static void LogException(string message, Exception exception)
            => Capture(CreateSentryEvent(exception, message));

        public static Task LogInfoAsync(string message)
            => CaptureAsync(CreateSentryEvent(message, false));

        public static void LogInfo(string message)
            => Capture(CreateSentryEvent(message, false));

        public static void LogInfo(string message, IDictionary<string, string> properties)
            => Capture(CreateSentryEvent(message, properties, false));

        private static SentryEvent CreateSentryEvent(Exception exception)
            => CreateSentryEvent(exception, string.Empty);

        private static SentryEvent CreateSentryEvent(Exception exception, string message, bool isErrorEvent = true)
            => DecorateSentryEvent(() => new SentryEvent(exception) { Message = message }, isErrorEvent);

        private static SentryEvent CreateSentryEvent(string message, bool isErrorEvent = true)
            => DecorateSentryEvent(() => new SentryEvent { Message = message }, isErrorEvent);

        private static SentryEvent CreateSentryEvent(string message, IDictionary<string, string> properties, bool isErrorEvent = true)
        {
            properties.Add("message", message);

            var logMessage = string.Join(Console.Out.NewLine, properties.Select(kv => $"{kv.Key}:{kv.Value}").ToArray());

            return DecorateSentryEvent(() => new SentryEvent { Message = logMessage }, isErrorEvent);
        }

        private static SentryEvent CreateSentryEvent(Exception exception, IDictionary<string, string> properties)
        {
            var logMessage = string.Join(Console.Out.NewLine, properties.Select(kv => $"{kv.Key}:{kv.Value}").ToArray());

            return DecorateSentryEvent(() => new SentryEvent(exception) { Message = logMessage });
        }

        private static SentryEvent DecorateSentryEvent(Func<SentryEvent> sentryEventCreator, bool isErrorEvent = true)
        {
            var sentryEvent = sentryEventCreator.Invoke();

            sentryEvent.Level = isErrorEvent
                ? SentryLevel.Error
                : SentryLevel.Info;

            sentryEvent.Contexts.Device.Model = DeviceInfo.Model;
            sentryEvent.Contexts.Device.Name = DeviceInfo.Name;
            sentryEvent.Contexts.Device.Simulator = (DeviceInfo.DeviceType == DeviceType.Virtual);
            sentryEvent.Contexts.Device.Manufacturer = DeviceInfo.Manufacturer;
            sentryEvent.Contexts.Runtime.Version = AppInfo.VersionString;
            sentryEvent.Contexts.Runtime.Build = AppInfo.BuildString;
            sentryEvent.Contexts.OperatingSystem.Name = DeviceInfo.Platform.ToString();
            sentryEvent.Contexts.OperatingSystem.Version = DeviceInfo.VersionString;
            sentryEvent.Platform = DeviceInfo.Platform.ToString();

            return sentryEvent;
        }

        private static void Capture(SentryEvent sentryEvent)
            => SentrySdk.CaptureEvent(sentryEvent);

        private static Task CaptureAsync(SentryEvent sentryEvent)
            => Task.Run(() => Capture(sentryEvent));
    }
}