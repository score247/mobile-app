namespace LiveScore.Core.Tests.Fixtures
{
    using LiveScore.Core.Enumerations;
    using LiveScore.Core.Services;
    using NSubstitute;

    public class AppSettingsFixture
    {
        public AppSettingsFixture()
        {
            SettingsService = Substitute.For<ISettingsService>();
            SettingsService.CurrentSportType.Returns(SportTypes.Soccer);
        }

        public ISettingsService SettingsService { get; }
    }
}