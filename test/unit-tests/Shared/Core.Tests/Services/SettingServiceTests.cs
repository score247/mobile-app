using LiveScore.Common.Services;
using LiveScore.Core.Constants;
using LiveScore.Core.Models.Settings;
using LiveScore.Core.Services;
using NSubstitute;
using System;
using Xunit;

namespace LiveScore.Core.Tests.Services
{
    public class SettingServiceTests
    {
        private readonly ICacheService mockCache;
        private readonly SettingsService setting;

        public SettingServiceTests()
        {
            mockCache = Substitute.For<ICacheService>();

            setting = new SettingsService(mockCache);
        }

        [Fact]
        public void CurrentSport_Default_ShouldReturnDefault()
        {
            // Arrange
            var expected = SportType.Soccer;
            mockCache.GetValueOrDefault(Arg.Is<string>(x => x.Equals("CurrentSport")), Arg.Any<SportType>()).Returns(SportType.Soccer);

            // Act
            var actual = setting.CurrentSport;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CurrentSport_AssignedSport_ShouldInjectCacheService()
        {
            // Arrange
            var expected = SportType.Basketball;

            // Act
            setting.CurrentSport = expected;

            // Assert
            mockCache.Received(1).AddOrUpdateValue(Arg.Is<string>(x => x.Equals("CurrentSport")), Arg.Any<object>());
        }

        [Fact]
        public void CurrentLanguage_Default_ShouldReturnDefault()
        {
            // Arrange
            var expected = LanguageCode.En.ToString();
            mockCache.GetValueOrDefault(Arg.Is<string>(x => x.Equals("CurrentLanguage")), Arg.Any<string>()).Returns(LanguageCode.En.ToString());

            // Act
            var actual = setting.CurrentLanguage;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CurrentLanguage_Assigned_ShouldInjectCacheService()
        {
            // Arrange
            var expected = LanguageCode.Vi.ToString();

            // Act
            setting.CurrentLanguage = expected;

            // Assert
            mockCache.Received(1).AddOrUpdateValue(Arg.Is<string>(x => x.Equals("CurrentLanguage")), Arg.Any<object>());
        }

        [Fact]
        public void CurrentTimeZone_Default_ShouldReturnDefault()
        {
            // Arrange
            var expected = TimeZoneInfo.Local;
            mockCache.GetValueOrDefault(Arg.Is<string>(x => x.Equals("CurrentTimeZone")), Arg.Any<TimeZoneInfo>()).Returns(expected);

            // Act
            var actual = setting.CurrentTimeZone;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CurrentTimeZone_Assigned_ShouldInjectCacheService()
        {
            // Arrange
            var expected = TimeZoneInfo.Local;

            // Act
            setting.CurrentTimeZone = expected;

            // Assert
            mockCache.Received(1).AddOrUpdateValue(Arg.Is<string>(x => x.Equals("CurrentTimeZone")), Arg.Any<object>());
        }

        [Fact]
        public void UserSettings_ShouldReturnCorrectly()
        {
            // Arrange
            var expected = new UserSettings((int)SportType.Soccer, LanguageCode.En.ToString(), TimeZoneInfo.Local.BaseUtcOffset.ToString());           
            mockCache.GetValueOrDefault(Arg.Is<string>(x => x.Equals("CurrentTimeZone")), Arg.Any<TimeZoneInfo>()).Returns(TimeZoneInfo.Local);
            mockCache.GetValueOrDefault(Arg.Is<string>(x => x.Equals("CurrentLanguage")), Arg.Any<string>()).Returns(LanguageCode.En.ToString());
            mockCache.GetValueOrDefault(Arg.Is<string>(x => x.Equals("CurrentSport")), Arg.Any<SportType>()).Returns(SportType.Soccer);

            // Act
            var actual = setting.UserSettings;

            // Assert
            Assert.Equal(expected.ToString(), actual.ToString());
        }
    }
}
