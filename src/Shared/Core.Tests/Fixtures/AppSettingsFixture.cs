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
            //SettingsService = Substitute.For<ISettingsService>();
            //SettingsService.UserSettings.SportId.Returns(SportType.Soccer.Value);
            ////SettingsService.UserSettings.Returns(new OldUserSettings(SportTypes.Soccer.DisplayName, "en-US", "7"));
        }

        public ISettingsService SettingsService { get; }
    }
}