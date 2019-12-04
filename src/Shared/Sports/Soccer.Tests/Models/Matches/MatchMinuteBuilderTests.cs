using System;
using AutoFixture;
using LiveScore.Common.Services;
using LiveScore.Core;
using LiveScore.Core.Enumerations;
using LiveScore.Soccer.Models.Matches;
using NSubstitute;
using Xunit;
using static Soccer.Tests.Services.Utils.MatchMinuteUtil;

namespace Soccer.Tests.Models.Matches
{
    public class MatchMinuteBuilderTests
    {
        private readonly ISettings settings;
        private readonly ILoggingService loggingService;
        private readonly MatchMinuteBuilder matchMinuteBuilder;
        private readonly Fixture fixture;

        public MatchMinuteBuilderTests()
        {
            settings = Substitute.For<ISettings>();
            loggingService = Substitute.For<ILoggingService>();
            matchMinuteBuilder = new MatchMinuteBuilder(settings, loggingService);

            fixture = new Fixture();
        }

        [Fact]
        public void BuildMatchMinute_TimePeriodKey_DoesNotContainMatchStatus_ReturnStringEmpty()
        {
            var actual = matchMinuteBuilder.BuildMatchMinute(null);

            Assert.Equal(string.Empty, actual);
        }

        [Fact]
        public void BuildMatchMinute_MatchIsNullOrNotASoccerMatch_ReturnStringEmpty()
        {
            // Act
            var actualDisplayMinute = matchMinuteBuilder.BuildMatchMinute(null, fixture.Create<DateTimeOffset>());

            // Assert
            Assert.Equal(string.Empty, actualDisplayMinute);
        }

        [Fact]
        public void BuildMatchMinute_MatchSatusIsFirstHalf_ReturnMatchTimeMinute()
        {
            // Arrange
            var matchResult = new MatchResult(fixture.Create<MatchStatus>(), MatchStatus.FirstHalf);
            var match = new SoccerMatch(fixture.Create<DateTime>(), matchResult);
            var expectedMinute = GenerateMatchMinute(matchResult.MatchStatus);
            var timeToViewMatch = match.EventDate.AddMinutes(expectedMinute);

            // Act
            var actualDisplayMinute = matchMinuteBuilder.BuildMatchMinute(match, timeToViewMatch);

            // Assert
            Assert.True(IsValidMatchMinuteDisplay(actualDisplayMinute, expectedMinute));
        }

        [Fact]
        public void BuildMatchMinute_MatchSatusIsSecondHalf_ReturnMatchTimeMinute()
        {
            // Arrange
            var matchResult = new MatchResult(fixture.Create<MatchStatus>(), MatchStatus.SecondHalf);

            var eventDate = fixture.Create<DateTime>();
            const byte minuteSpentForBreakTime = 15;
            const byte minutesSpentForFirstHalf = 47;
            var (_, periodEndMinute) = PeriodTimes[MatchStatus.FirstHalf];
            var timeToStartSecondHalf = eventDate
                .AddMinutes(minutesSpentForFirstHalf)
                .AddMinutes(minuteSpentForBreakTime);

            var match = new SoccerMatch(eventDate, matchResult);
            match.CurrentPeriodStartTime = timeToStartSecondHalf;

            var expectedMinute = GenerateMatchMinute(match.MatchStatus);
            var minuteToViewMatchAfterSecondHalfStarts = match
                .CurrentPeriodStartTime
                .AddMinutes(expectedMinute - periodEndMinute);

            // Act
            var actualDisplayMinute = matchMinuteBuilder.BuildMatchMinute(match, minuteToViewMatchAfterSecondHalfStarts);

            // Assert

            Assert.True(IsValidMatchMinuteDisplay(actualDisplayMinute, expectedMinute));
        }
    }
}