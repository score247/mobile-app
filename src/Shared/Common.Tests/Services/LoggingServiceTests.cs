﻿namespace LiveScore.Common.Tests.Services
{
    using System;
    using System.Threading.Tasks;
    using LiveScore.Common.Services;
    using NSubstitute;
    using SharpRaven;
    using SharpRaven.Data;
    using Xunit;

    public class LoggingServiceTests
    {
        private readonly IEssential mockEssentials;
        private readonly IRavenClient mockRavenClient;
        private readonly ILoggingService loggingService;
        private readonly INetworkConnection mockNetworkManager;

        public LoggingServiceTests()
        {
            mockEssentials = new MockEssentials();
            mockRavenClient = Substitute.For<IRavenClient>();
            mockNetworkManager = Substitute.For<INetworkConnection>();

            loggingService = new LoggingService(mockEssentials, mockNetworkManager, mockRavenClient, "dns", "env");
        }

        [Fact]
        public void LogError_WhenCall_InjectCaptureEvent()
        {
            // Arrange
            mockNetworkManager.IsSuccessfulConnection().Returns(true);
            var exception = new InvalidOperationException("");

            // Act
            loggingService.LogException(exception);

            // Assert
            mockRavenClient.Received(1).Capture(
                Arg.Is<SentryEvent>(s => s.Exception == exception));
        }

        [Fact]
        public void LogError_WhenCall_CorrectDeviceName()
        {
            // Arrange
            mockNetworkManager.IsSuccessfulConnection().Returns(true);

            // Act
            loggingService.LogException(new InvalidOperationException(""));

            // Assert
            mockRavenClient.Received(1).Capture(Arg.Is<SentryEvent>(x => x.Contexts.Device.Name.Equals("iPhone")));
        }

        [Fact]
        public void LogError_WhenCall_CorrectDeviceModel()
        {
            // Arrange
            mockNetworkManager.IsSuccessfulConnection().Returns(true);

            // Act
            loggingService.LogException(new InvalidOperationException(""));

            // Assert
            mockRavenClient.Received(1).Capture(Arg.Is<SentryEvent>(x => x.Contexts.Device.Model.Equals("6")));
        }

        [Fact]
        public void LogError_WhenCall_CorrectOperatingSystemName()
        {
            // Arrange
            mockNetworkManager.IsSuccessfulConnection().Returns(true);

            // Act
            loggingService.LogException(new InvalidOperationException(""));

            // Assert
            mockRavenClient.Received(1).Capture(Arg.Is<SentryEvent>(x => x.Contexts.OperatingSystem.Name.Equals("iOS")));
        }

        [Fact]
        public void LogError_WhenCall_CorrectOperatingSystemVersion()
        {
            // Arrange
            mockNetworkManager.IsSuccessfulConnection().Returns(true);

            // Act
            loggingService.LogException(new InvalidOperationException(""));

            // Assert
            mockRavenClient.Received(1).Capture(Arg.Is<SentryEvent>(x => x.Contexts.OperatingSystem.Version.Equals("11.2")));
        }

        [Fact]
        public async Task LogErrorAsync_WhenCall_InjectCaptureEventAsync()
        {
            // Arrange
            mockNetworkManager.IsSuccessfulConnection().Returns(true);

            // Act
            await loggingService.LogExceptionAsync(new InvalidOperationException(""));

            // Assert
            await mockRavenClient.Received(1).CaptureAsync(Arg.Any<SentryEvent>());
        }
    }

    public class MockEssentials : IEssential
    {
        public string Model => "6";

        public string Name => "iPhone";

        public string OperatingSystemName => "iOS";

        public string OperatingSystemVersion => "11.2";

        public string AppVersion => "1.2";

        public string AppName => "Score247";
    }
}