using System;
using AutoFixture;
using LiveScore.Common.Services;
using LiveScore.Core;
using LiveScore.Core.Enumerations;
using LiveScore.Soccer.Models.Matches;
using NSubstitute;
using Soccer.Tests.Services.Utils;
using Xunit;

namespace Soccer.Tests.Converters
{
    public class MatchMinuteConverterTests
    {
        private readonly ISettings settings;
        private readonly ILoggingService loggingService;
        private readonly MatchMinuteBuilder matchMinuteConverter;
        private readonly Fixture Random;

        public MatchMinuteConverterTests()
        {
            settings = Substitute.For<ISettings>();
            loggingService = Substitute.For<ILoggingService>();
            matchMinuteConverter = new MatchMinuteBuilder(settings, loggingService);

            Random = new Fixture();
        }

        [Fact]
        public void BuildMatchMinute_MatchIsNullOrNotASoccerMatch_ReturnStringEmpty()
        {
            var actual = matchMinuteConverter.BuildMatchMinute(null);

            Assert.Equal(string.Empty, actual);
        }

        [Fact]
        public void BuildMatchMinute_MatchSatusIsFirstHalf_ReturnMatchTimeMinute()
        {
            // Arrange
            var matchResult = new MatchResult(Random.Create<MatchStatus>(), MatchStatus.FirstHalf);
            var expectedMatchMinuteToSee = MatchMinuteGenerator.RandomMinuteFor(matchResult.MatchStatus);
            var acceptableMinuteToSee = expectedMatchMinuteToSee + 1;
            var eventDate = Random.Create<DateTime>();
            var timeToViewMatch = new DateTimeOffset(eventDate).AddMinutes(expectedMatchMinuteToSee);
            var match = new SoccerMatch(eventDate, matchResult);

            // Act
            var actualMatchMinuteToDisplay = matchMinuteConverter.BuildMatchMinute(match, timeToViewMatch);

            // Assert
            var result = actualMatchMinuteToDisplay.Equals($"{acceptableMinuteToSee}'", StringComparison.OrdinalIgnoreCase)
                || actualMatchMinuteToDisplay.Equals($"{expectedMatchMinuteToSee}'", StringComparison.OrdinalIgnoreCase);

            Assert.True(result);
        }

        [Fact]
        public void BuildMatchMinute_MatchSatusIsSecondtHalf_ReturnMatchTimeMinute()
        {
            // Arrange
            var matchResult = new MatchResult(Random.Create<MatchStatus>(), MatchStatus.SecondHalf);
            var expectedMatchMinuteToSee = MatchMinuteGenerator.RandomMinuteFor(matchResult.MatchStatus);
            var acceptableMinuteToSee = expectedMatchMinuteToSee + 1;
            var eventDate = Random.Create<DateTime>();
            var match = new SoccerMatch(eventDate, matchResult)
            {
                CurrentPeriodStartTime = eventDate.AddMinutes(45).AddMinutes(16)
            };

            var timeToViewMatch = match.CurrentPeriodStartTime.AddMinutes(expectedMatchMinuteToSee - 46);

            // Act
            var actualMatchMinuteToDisplay = matchMinuteConverter.BuildMatchMinute(match, timeToViewMatch);

            // Assert
            var result = actualMatchMinuteToDisplay.Equals($"{acceptableMinuteToSee}'", StringComparison.OrdinalIgnoreCase)
                || actualMatchMinuteToDisplay.Equals($"{expectedMatchMinuteToSee}'", StringComparison.OrdinalIgnoreCase);

            Assert.True(result);
        }
    }
}