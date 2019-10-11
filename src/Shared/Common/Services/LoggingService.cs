using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SharpRaven;
using SharpRaven.Data;
using Xamarin.Essentials;

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

        void TrackEvent(string trackIdentifier, string key, string value);
    }

    public class LoggingService : ILoggingService
    {
        private readonly IEssential deviceInfo;
        private readonly INetworkConnection networkConnectionManager;
        private readonly IRavenClient ravenClient;

        public LoggingService(
            IEssential deviceInfo,
            INetworkConnection networkConnectionManager,
            IRavenClient ravenClient,
            string dns,
            string env)
        {
            this.deviceInfo = deviceInfo;
            this.networkConnectionManager = networkConnectionManager;
            this.ravenClient = ravenClient?? new RavenClient(dns)
            {
                Release = AppInfo.VersionString,
                Environment = env
            };
        }

        public Task LogExceptionAsync(Exception exception)
            => CaptureAsync(CreateSentryEvent(exception));

        public Task LogExceptionAsync(string message, Exception exception)
            => CaptureAsync(CreateSentryEvent(message, exception));

        public void LogException(Exception exception)
            => Capture(CreateSentryEvent(exception));

        public void LogException(string message, Exception exception)
            => Capture(CreateSentryEvent(exception));

        public Task LogInfoAsync(string message)
            => CaptureAsync(CreateSentryEvent(message, ErrorLevel.Info));

        public void LogInfo(string message)
            => Capture(CreateSentryEvent(message, ErrorLevel.Info));

        private SentryEvent CreateSentryEvent(Exception exception)
           => CreateSentryEvent(string.Empty, exception);

        private SentryEvent CreateSentryEvent(string message, Exception exception)
         => CreateSentryEvent(() => new SentryEvent(exception)
         {
             Message = new SentryMessage((message ?? string.Empty) + exception.Message),
             Level = ErrorLevel.Error
         });

        private SentryEvent CreateSentryEvent(string message, ErrorLevel errorLevel)
            => CreateSentryEvent(() => new SentryEvent(deviceInfo?.Name + " " + message)
            {
                Level = errorLevel
            });

        private SentryEvent CreateSentryEvent(Func<SentryEvent> sentryEventCreator)
        {
            var sentryEvent = sentryEventCreator.Invoke();

            sentryEvent.Contexts.Device.Model = deviceInfo.Model;
            sentryEvent.Contexts.Device.Name = deviceInfo.Name;
            sentryEvent.Contexts.OperatingSystem.Name = deviceInfo.OperatingSystemName;
            sentryEvent.Contexts.OperatingSystem.Version = deviceInfo.OperatingSystemVersion;

            return sentryEvent;
        }

        private void Capture(SentryEvent sentryEvent)
        {
            // TODO: when network is down, record event
            if (networkConnectionManager.IsSuccessfulConnection())
            {
                ravenClient?.Capture(sentryEvent);
            }
        }

        private Task CaptureAsync(SentryEvent sentryEvent)
        {
            if (networkConnectionManager.IsSuccessfulConnection())
            {
                return ravenClient?.CaptureAsync(sentryEvent);
            }

            return Task.CompletedTask;
        }

        private void TrackEvent(string trackIdentifier, IDictionary<string, string> table)
            => ravenClient?.AddTrail(
                new Breadcrumb(trackIdentifier)
                {
                    Data = table
                });

        public void TrackEvent(string trackIdentifier, string key, string value)
            => TrackEvent(trackIdentifier, new Dictionary<string, string>
            {
                { key, value }
            });
    }
}