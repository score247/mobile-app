namespace LiveScore.Common.Tests.Services
{
    public class LoggingServiceTests
    {
        //private readonly ILoggingService loggingService;

        //private readonly Action<Exception, IDictionary<string, string>> trackError;
        //private readonly Action<string, IDictionary<string, string>> trackEvent;
        //private readonly Fixture fixture;

        //public LoggingServiceTests()
        //{
        //    fixture = new Fixture();

        //    trackError = Substitute.For<Action<Exception, IDictionary<string, string>>>();
        //    trackEvent = Substitute.For<Action<string, IDictionary<string, string>>>();

        //    loggingService = new LoggingService(trackError, trackEvent);
        //}

        //[Fact]
        //public void LogException_Always_Invoke_TrackerErrorAction_With_ClientInformation()
        //{
        //    // Arrange
        //    var exception = fixture.Create<Exception>();

        //    // Act
        //    loggingService.LogException(exception);

        //    // Assert
        //    trackError
        //        .Received()
        //        .Invoke(exception, null);
        //}

        //[Fact]
        //public void LogException_With_CustomMessage_Call_Tracker_Error_With_TheMessage()
        //{
        //    // Arrange
        //    var message = fixture.Create<string>();
        //    var exception = fixture.Create<Exception>();

        //    // Act
        //    loggingService.LogException(exception, message);

        //    // Assert
        //    trackError
        //        .Received()
        //        .Invoke(exception, Arg.Is<Dictionary<string, string>>(
        //            arg => arg["message"] == message));
        //}

        //[Fact]
        //public void TrackEvent_Always_Invoke_TrackEventAction_Function_With_ClientInformation()
        //{
        //    // Arrange
        //    var message = fixture.Create<string>();
        //    var trackIndentifier = fixture.Create<string>();

        //    // Act
        //    loggingService.TrackEvent(trackIndentifier, message);

        //    // Assert
        //    trackEvent
        //        .Received()
        //        .Invoke(trackIndentifier, Arg.Is<Dictionary<string, string>>(
        //            arg => arg["message"] == message));
        //}
    }
}