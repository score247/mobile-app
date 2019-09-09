namespace LiveScore.Core.Tests.Fixtures
{
    using LiveScore.Core.Enumerations;
    using LiveScore.Core.Services;
    using NSubstitute;

    public class AppSettingsFixture
    {
        public AppSettingsFixture()
        {
            AppSettings = Substitute.For<IAppSettings>();
            AppSettings.CurrentSportType.Returns(SportType.Soccer);
            AppSettings.CurrentLanguage.Returns(Language.English);
        }

        public IAppSettings AppSettings { get; }
    }
}