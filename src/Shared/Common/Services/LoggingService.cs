using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        Task LogInfoAsync(string message);

        void LogInfo(string message);

        void Init(string Dsn, string env = "", IRavenClient ravenClient = null);
    }

    public class LoggingService : ILoggingService
    {
        private readonly IEssential deviceInfo;
        private readonly INetworkConnection networkConnectionManager;

        private IRavenClient ravenClient;

        public LoggingService(IEssential deviceInfo, INetworkConnection networkConnectionManager)
        {
            this.deviceInfo = deviceInfo;
            this.networkConnectionManager = networkConnectionManager;
        }

        public void Init(string Dsn, string env = "", IRavenClient ravenClient = null)
        {
            this.ravenClient = ravenClient ?? new RavenClient(Dsn) { Release = deviceInfo.AppVersion, Environment = env };

            LogInfoAsync($"Init Logging Service with DSN {Dsn} env {env}");
        }

        public void LogError(Exception exception)
        {
            Debug.WriteLine("LogError");

            if (networkConnectionManager.IsSuccessfulConnection())
            {
                ravenClient?.Capture(CreateSentryEvent(string.Empty, exception));
            }
        }

        public void LogError(string message, Exception exception)
        {
            if (networkConnectionManager.IsSuccessfulConnection())
            {
                ravenClient?.Capture(CreateSentryEvent(message, exception));
            }
        }

        public Task LogErrorAsync(Exception exception)
        {
            if (networkConnectionManager.IsSuccessfulConnection())
            {
                return ravenClient?.CaptureAsync(CreateSentryEvent(string.Empty, exception));
            }

            return Task.CompletedTask;
        }

        public Task LogErrorAsync(string message, Exception exception)
        {
            if (networkConnectionManager.IsSuccessfulConnection())
            {
                return ravenClient?.CaptureAsync(CreateSentryEvent(message, exception));
            }

            return Task.CompletedTask;
        }

        public Task LogInfoAsync(string message)
        {
            if (networkConnectionManager.IsSuccessfulConnection())
            {
                return ravenClient?.CaptureAsync(CreateSentryInfoEvent(message));
            }

            return Task.CompletedTask;
        }

        public void LogInfo(string message)
        {
            if (networkConnectionManager.IsSuccessfulConnection())
            {
                ravenClient?.Capture(CreateSentryInfoEvent(message));
            }
        }

        private SentryEvent CreateSentryEvent(string message, Exception exception)
        {
            var evt = new SentryEvent(exception);

            if (!string.IsNullOrWhiteSpace(message))
            {
                evt.Message = new SentryMessage(deviceInfo?.Name + " " + message);
            }

            evt.Contexts.Device.Model = deviceInfo.Model;
            evt.Contexts.Device.Name = deviceInfo.Name;
            evt.Contexts.OperatingSystem.Name = deviceInfo.OperatingSystemName;
            evt.Contexts.OperatingSystem.Version = deviceInfo.OperatingSystemVersion;

            return evt;
        }

        private SentryEvent CreateSentryInfoEvent(string message)
        {
            var evt = new SentryEvent(deviceInfo?.Name + " " + message)
            {
                Level = ErrorLevel.Info
            };

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