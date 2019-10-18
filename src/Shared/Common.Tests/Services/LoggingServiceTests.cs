namespace LiveScore.Common.Tests.Services
{
    using LiveScore.Common.Services;

    public class LoggingServiceTests
    {
        private readonly IEssential mockEssentials;
        private readonly ILoggingService loggingService;
        private readonly INetworkConnection mockNetworkManager;
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