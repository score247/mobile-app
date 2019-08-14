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
            SettingsService = Substitute.For<IApplicationContext>();
            SettingsService.SportId.Returns(SportType.Soccer);
            //SettingsService.UserSettings.Returns(new OldUserSettings(SportTypes.Soccer.DisplayName, "en-US", "7"));
        }

        public IApplicationContext SettingsService { get; }
    }
}