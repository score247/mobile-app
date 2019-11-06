using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Sentry;

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
        private const string MoreInfo = "more info";
        private readonly Action<Exception, IDictionary<string, string>> trackError;
        private readonly Action<string, IDictionary<string, string>> trackEvent;

        public LoggingService(
            Action<Exception, IDictionary<string, string>> trackError = null,
            Action<string, IDictionary<string, string>> trackEvent = null,
            Func<bool> isSentryEnable = null)
        {
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
        {
            LogException(exception, new Dictionary<string, string>
            {
                [MoreInfo] = message
            });
        }

        public void LogException(Exception exception, IDictionary<string, string> properties)
        {
            trackError(exception, properties);
        }

        public Task LogExceptionAsync(Exception exception)
            => Task.Run(() => LogException(exception));

        public Task LogExceptionAsync(Exception exception, string message)
            => Task.Run(() => LogException(exception, message));

        public Task LogExceptionAsync(Exception exception, IDictionary<string, string> properties)
            => Task.Run(() => LogException(exception, properties));

        public void TrackEvent(string trackIdentifier, IDictionary<string, string> properties)
        {
            trackEvent(trackIdentifier, properties);
        }

        public void TrackEvent(string trackIdentifier, string message)
            => TrackEvent(trackIdentifier, new Dictionary<string, string>
            {
                [MoreInfo] = message
            });

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

        private static SentryEvent CreateSentryEvent(Exception exception, string message, bool isError = true)
         => CreateSentryEvent(() => new SentryEvent(exception)
         {
             Message = (message ?? string.Empty) + exception.Message,
             Level = isError ? Sentry.Protocol.SentryLevel.Error : Sentry.Protocol.SentryLevel.Info
         });

        private static SentryEvent CreateSentryEvent(string message, bool isError = true)
            => CreateSentryEvent(() => new SentryEvent
            {
                Message = message,
                Level = isError ? Sentry.Protocol.SentryLevel.Error : Sentry.Protocol.SentryLevel.Info
            });

        private static SentryEvent CreateSentryEvent(string message, IDictionary<string, string> properties, bool isError = true)
        {
            properties.Add("message", message);

            var logMessage = string.Join(Console.Out.NewLine, properties.Select(kv => $"{kv.Key}:{kv.Value}").ToArray());

            return CreateSentryEvent(() => new SentryEvent
            {
                Message = logMessage,
                Level = isError ? Sentry.Protocol.SentryLevel.Error : Sentry.Protocol.SentryLevel.Info
            });
        }

        private static SentryEvent CreateSentryEvent(Exception exception, IDictionary<string, string> properties)
        {
            var message = string.Join(Console.Out.NewLine, properties.Select(kv => $"{kv.Key}:{kv.Value}").ToArray());

            return CreateSentryEvent(() => new SentryEvent(exception) { Message = message });
        }

        private static SentryEvent CreateSentryEvent(Func<SentryEvent> sentryEventCreator)
        {
            var sentryEvent = sentryEventCreator.Invoke();

            sentryEvent.Contexts.Device.Model = Xamarin.Essentials.DeviceInfo.Model;
            sentryEvent.Contexts.Device.Name = Xamarin.Essentials.DeviceInfo.Name;
            sentryEvent.Contexts.Device.Manufacturer = Xamarin.Essentials.DeviceInfo.Manufacturer;
            sentryEvent.Contexts.Runtime.Version = Xamarin.Essentials.VersionTracking.CurrentVersion;
            sentryEvent.Contexts.Runtime.Build = Xamarin.Essentials.VersionTracking.CurrentBuild;

            sentryEvent.Contexts.OperatingSystem.Name = Xamarin.Essentials.DeviceInfo.Platform.ToString();
            sentryEvent.Contexts.OperatingSystem.Version = Xamarin.Essentials.DeviceInfo.VersionString;

            return sentryEvent;
        }

        private static void Capture(SentryEvent sentryEvent)
        {
            SentrySdk.CaptureEvent(sentryEvent);
        }

        private static Task CaptureAsync(SentryEvent sentryEvent)
        {
            return Task.Run(() => Capture(sentryEvent));
        }
    }
}