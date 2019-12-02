using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AutoFixture;
using LiveScore.Common.Services;
using NSubstitute;
using Refit;
using Xunit;

namespace LiveScore.Common.Tests.Services
{
    public class LoggingServiceTests
    {
        private readonly ILoggingService loggingService;
        private readonly string KeyEmpty = string.Empty;
        private readonly Action<Exception, IDictionary<string, string>> trackError;
        private readonly Action<string, IDictionary<string, string>> trackEvent;
        private readonly Func<bool> isSentryEnable;
        private readonly Fixture fixture;
        private readonly INetworkConnection networkConnection;

        public LoggingServiceTests()
        {
            fixture = new Fixture();

            trackError = Substitute.For<Action<Exception, IDictionary<string, string>>>();
            trackEvent = Substitute.For<Action<string, IDictionary<string, string>>>();
            networkConnection = Substitute.For<INetworkConnection>();
            networkConnection.IsSuccessfulConnection().Returns(true);
            isSentryEnable = () => true;
            loggingService = new LoggingService(networkConnection, trackError, trackEvent, isSentryEnable);
        }

        [Fact]
        public void LogException_Always_InvokeTrackErrorWithParamIsDictionaryContainsEmptyMessage()
        {
            // Arrange
            var exception = fixture.Create<Exception>();

            // Act
            loggingService.LogException(exception);

            // Assert
            trackError
                .Received()
                .Invoke(exception, Arg.Is<Dictionary<string, string>>(
                    arg => arg[KeyEmpty] == string.Empty));
        }

        [Fact]
        public void LogException_ParamIsMessage_InvokeTrackErrorWithParamIsTheMessage()
        {
            // Arrange
            var message = fixture.Create<string>();
            var exception = fixture.Create<Exception>();

            // Act
            loggingService.LogException(exception, message);

            // Assert
            trackError
                .Received()
                .Invoke(exception, Arg.Is<IDictionary<string, string>>(
                    arg => arg[KeyEmpty] == message));
        }

        [Fact]
        public void LogException_ParamIsDictionary_InvokeTrackErrorWithParamIsDictionaryContainsTheMessage()
        {
            // Arrange
            var properties = fixture.Create<IDictionary<string, string>>();
            var exception = fixture.Create<Exception>();

            // Act
            loggingService.LogException(exception, properties);

            // Assert
            trackError
                .Received()
                .Invoke(exception, properties);
        }

        [Fact]
        public async Task LogExceptionAsync_ParamIsException_InvokeTrackErrorWithParamIsDictionaryContainsEmptyMessage()
        {
            // Arrange
            var exception = fixture.Create<Exception>();

            // Act
            await loggingService.LogExceptionAsync(exception).ConfigureAwait(false);

            // Assert
            trackError
               .Received()
               .Invoke(exception, Arg.Is<Dictionary<string, string>>(
                   arg => arg[KeyEmpty] == string.Empty));
        }

        [Fact]
        public async Task LogExceptionAsync_ParamIsMessage_InvokeTrackErrorWithParamIsDictionaryContainsTheMessage()
        {
            // Arrange
            var message = fixture.Create<string>();
            var exception = fixture.Create<Exception>();

            // Act
            await loggingService
                .LogExceptionAsync(exception, message)
                .ConfigureAwait(false);

            // Assert
            trackError
                .Received()
                .Invoke(exception, Arg.Is<Dictionary<string, string>>(
                    arg => arg[KeyEmpty] == message));
        }

        [Fact]
        public async Task LogExceptionAsync_ParamIsDictionary_InvokeTrackErrorWithParamIsTheDictionary()
        {
            // Arrange
            var properties = fixture.Create<IDictionary<string, string>>();
            var exception = fixture.Create<Exception>();

            // Act
            await loggingService
                .LogExceptionAsync(exception, properties)
                .ConfigureAwait(false);

            // Assert
            trackError
                .Received()
                .Invoke(exception, properties);
        }

        [Fact]
        public void TrackEvent_ParamIsMessage_InvokeTrackEventWithParamIsTheMessage()
        {
            // Arrange
            var message = fixture.Create<string>();
            var trackIndentifier = fixture.Create<string>();

            // Act
            loggingService.TrackEvent(trackIndentifier, message);

            // Assert
            trackEvent
                .Received()
                .Invoke(trackIndentifier, Arg.Is<Dictionary<string, string>>(
                    arg => arg[KeyEmpty] == message));
        }



        [Fact]
        public void TrackEvent_ParamIsDictionary_InvokeTrackEventWithParamIsDictionary()
        {
            // Arrange
            var properties = fixture.Create<IDictionary<string, string>>();
            var trackIndentifier = fixture.Create<string>();

            // Act
            loggingService.TrackEvent(trackIndentifier, properties);

            // Assert
            trackEvent
                .Received()
                .Invoke(trackIndentifier, properties);
        }

        [Fact]
        public async Task TrackEventAsync_ParamIsMessage_InvokeTrackEventWithParamIsTheMessage()
        {
            // Arrange
            var message = fixture.Create<string>();
            var trackIndentifier = fixture.Create<string>();

            // Act
            await loggingService.TrackEventAsync(trackIndentifier, message);

            // Assert
            trackEvent
                .Received()
                .Invoke(trackIndentifier, Arg.Is<Dictionary<string, string>>(
                    arg => arg[KeyEmpty] == message));
        }



        [Fact]
        public async Task TrackEventAsync_ParamIsDictionary_InvokeTrackEventWithParamIsDictionary()
        {
            // Arrange
            var properties = fixture.Create<IDictionary<string, string>>();
            var trackIndentifier = fixture.Create<string>();

            // Act
            await loggingService.TrackEventAsync(trackIndentifier, properties);

            // Assert
            trackEvent
                .Received()
                .Invoke(trackIndentifier, properties);
        }

        [Fact]
        public async Task LogExceptionAsync_ExceptionIsApiException_InvokeTrackErrorWithApiInformation()
        {
            // Arrange
            var properties = fixture.Create<IDictionary<string, string>>();
            var exception = await ApiException.Create(
                                new HttpRequestMessage { 
                                    RequestUri = new Uri("http://demo.com")
                                },
                                HttpMethod.Get,
                                new HttpResponseMessage(HttpStatusCode.InternalServerError));

            // Act
            await loggingService.LogExceptionAsync(exception, properties);

            // Assert
            trackError
                .Received()
                .Invoke(exception, Arg.Is<Dictionary<string,string>>(
                        properties => properties.ContainsKey("Api Exception") 
                                        && properties["Api Exception"] == string.Join("\r\n", 
                                                                                "Request URL: http://demo.com/",
                                                                                "Response: ",
                                                                                "Reason Phrase: Internal Server Error")));
        }
    }
}