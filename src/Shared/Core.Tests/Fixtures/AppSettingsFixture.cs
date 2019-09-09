namespace LiveScore.Core.Tests.Fixtures
{
    using LiveScore.Core.Enumerations;
    using LiveScore.Core.Services;
    using NSubstitute;

    public class AppSettingsFixture
    {
        public AppSettingsFixture()
        {
            Settings = Substitute.For<ISettings>();
            Settings.CurrentSportType.Returns(SportType.Soccer);
            Settings.CurrentLanguage.Returns(Language.English);
        }

        public ISettings Settings { get; }
    }
}