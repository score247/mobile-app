namespace LiveScore.Core.Tests.Services
{
    public class SettingServiceTests
    {
        //private readonly ICachingService mockCache;
        //private readonly SettingsService setting;

        //public SettingServiceTests()
        //{
        //    mockCache = Substitute.For<ICachingService>();

        //    setting = new SettingsService(mockCache);
        //}

        //[Fact]
        //public void CurrentSport_Default_ShouldReturnDefault()
        //{
        //    // Arrange
        //    var expected = SportTypes.Soccer;
        //    mockCache.GetValueOrDefaultFromUserAccount(Arg.Is<string>(x => x.Equals(nameof(setting.CurrentSportType))), Arg.Any<SportTypes>()).Returns(SportTypes.Soccer);

        //    // Act
        //    var actual = setting.CurrentSportType;

        //    // Assert
        //    Assert.Equal(expected, actual);
        //}

        //[Fact]
        //public void CurrentSport_AssignedSport_ShouldInjectCacheService()
        //{
        //    // Arrange
        //    var expected = SportTypes.Basketball;

        //    // Act
        //    setting.CurrentSportType = expected;

        //    // Assert
        //    mockCache.Received(1).AddOrUpdateValueToUserAccount(Arg.Is<string>(x => x.Equals(nameof(setting.CurrentSportType))), Arg.Any<object>());
        //}

        //[Fact]
        //public void CurrentLanguage_Default_ShouldReturnDefault()
        //{
        //    // Arrange
        //    var expected = Languages.English.DisplayName;
        //    mockCache.GetValueOrDefaultFromUserAccount(Arg.Is<string>(x => x.Equals(nameof(setting.CurrentLanguage))), Arg.Any<string>()).Returns(Languages.English.DisplayName);

        //    // Act
        //    var actual = setting.CurrentLanguage;

        //    // Assert
        //    Assert.Equal(expected, actual);
        //}

        //[Fact]
        //public void CurrentLanguage_Assigned_ShouldInjectCacheService()
        //{
        //    // Arrange
        //    var expected = Languages.Vietnamese.DisplayName;

        //    // Act
        //    setting.CurrentLanguage = expected;

        //    // Assert
        //    mockCache.Received(1).AddOrUpdateValueToUserAccount(Arg.Is<string>(x => x.Equals(nameof(setting.CurrentLanguage))), Arg.Any<object>());
        //}

        //[Fact]
        //public void CurrentTimeZone_Default_ShouldReturnDefault()
        //{
        //    // Arrange
        //    var expected = TimeZoneInfo.Local;
        //    mockCache.GetValueOrDefaultFromUserAccount(Arg.Is<string>(x => x.Equals(nameof(setting.CurrentTimeZone))), Arg.Any<TimeZoneInfo>()).Returns(expected);

        //    // Act
        //    var actual = setting.CurrentTimeZone;

        //    // Assert
        //    Assert.Equal(expected, actual);
        //}

        //[Fact]
        //public void CurrentTimeZone_Assigned_ShouldInjectCacheService()
        //{
        //    // Arrange
        //    var expected = TimeZoneInfo.Local;

        //    // Act
        //    setting.CurrentTimeZone = expected;

        //    // Assert
        //    mockCache.Received(1).AddOrUpdateValueToUserAccount(Arg.Is<string>(x => x.Equals(nameof(setting.CurrentTimeZone))), Arg.Any<object>());
        //}

        //[Fact]
        //public void UserSettings_ShouldReturnCorrectly()
        //{
        //    // Arrange
        //    var expected = new UserSettings(SportTypes.Soccer.DisplayName, Languages.English.DisplayName, TimeZoneInfo.Local.BaseUtcOffset.ToString());
        //    mockCache.GetValueOrDefaultFromUserAccount(Arg.Is<string>(x => x.Equals(nameof(setting.CurrentTimeZone))), Arg.Any<TimeZoneInfo>()).Returns(TimeZoneInfo.Local);
        //    mockCache.GetValueOrDefaultFromUserAccount(Arg.Is<string>(x => x.Equals(nameof(setting.CurrentLanguage))), Arg.Any<string>()).Returns(Languages.English.DisplayName);
        //    mockCache.GetValueOrDefaultFromUserAccount(Arg.Is<string>(x => x.Equals(nameof(setting.CurrentSportType))), Arg.Any<SportTypes>()).Returns(SportTypes.Soccer);

        //    // Act
        //    var actual = setting.UserSettings;

        //    // Assert
        //    Assert.Equal(expected.ToString(), actual.ToString());
        //}
    }
}
