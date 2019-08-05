namespace LiveScore.Core.Tests.Fixtures
{
    using LiveScore.Core.Enumerations;
    using LiveScore.Core.Models.Settings;
    using LiveScore.Core.Services;
    using NSubstitute;

    public class AppSettingsFixture
    {
        public AppSettingsFixture()
        {
            SettingsService = Substitute.For<ISettingsService>();
            SettingsService.CurrentSportType.Returns(SportTypes.Soccer);
            SettingsService.UserSettings.Returns(new UserSettings(SportTypes.Soccer.DisplayName, "en-US", "7"));
        }

        public ISettingsService SettingsService { get; }
    }
}