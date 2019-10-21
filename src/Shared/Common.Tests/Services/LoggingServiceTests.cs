using System;
using System.Collections.Generic;
using AutoFixture;
using LiveScore.Common.Services;
using NSubstitute;
using Xunit;

namespace LiveScore.Common.Tests.Services
{
    public class LoggingServiceTests
    {
        private readonly ILoggingService loggingService;
        private readonly IEssential essentials;
        private readonly Action<Exception, IDictionary<string, string>> trackError;
        private readonly Action<string, IDictionary<string, string>> trackEvent;
        private readonly IDictionary<string, string> properties;
        private readonly Fixture fixture;

        public LoggingServiceTests()
        {
            essentials = Substitute.For<IEssential>();
            trackError = Substitute.For<Action<Exception, IDictionary<string, string>>>();
            trackEvent = Substitute.For<Action<string, IDictionary<string, string>>>();

            loggingService = new LoggingService(essentials, trackError, trackEvent);

            properties = GenerateClientInfo(essentials);

             fixture = new Fixture();
        }

        private static Dictionary<string, string> GenerateClientInfo(IEssential essential) => new Dictionary<string, string>
        {
            ["AppName"] = essential.AppName,
            ["AppVersion"] = essential.AppVersion,
            ["Device.Model"] = essential.Model,
            ["Device.Name"] = essential.Name,
            ["OperatingSystem.Name"] = essential.OperatingSystemName,
            ["OperatingSystem.Version"] = essential.OperatingSystemVersion,
            ["Message"] = string.Empty
        };

        [Fact]
        public void LogException_Always_Call_Tracker_Error_With_ClientInformation()
        {
            // Arrange
            var exception = fixture.Create<Exception>();

            // Act
            loggingService.LogException(exception);

            // Assert
            trackError
                .Received()
                .Invoke(exception, Arg.Is<Dictionary<string, string>>(
                    arg => arg["AppName"] == properties["AppName"]
                    && arg["AppVersion"] == properties["AppVersion"]
                    && arg["Device.Model"] == properties["Device.Model"]
                    && arg["Device.Name"] == properties["Device.Name"]
                    && arg["OperatingSystem.Name"] == properties["OperatingSystem.Name"]
                    && arg["OperatingSystem.Version"] == properties["OperatingSystem.Version"]
                    && arg["Message"] == string.Empty));
        }

        [Fact]
        public void LogException_With_CustomMessage_Call_Tracker_Error_With_TheMessage()
        {
            // Arrange
            var message = fixture.Create<string>();
            var exception = fixture.Create<Exception>();

            // Act
            loggingService.LogException(message, exception);

            // Assert
            trackError
                .Received()
                .Invoke(exception, Arg.Is<Dictionary<string, string>>(
                    arg => arg["AppName"] == properties["AppName"]
                    && arg["AppVersion"] == properties["AppVersion"]
                    && arg["Device.Model"] == properties["Device.Model"]
                    && arg["Device.Name"] == properties["Device.Name"]
                    && arg["OperatingSystem.Name"] == properties["OperatingSystem.Name"]
                    && arg["OperatingSystem.Version"] == properties["OperatingSystem.Version"]
                    && arg["Message"] == message));
        }
    }
}