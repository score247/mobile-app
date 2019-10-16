using System;
using AutoFixture;
using LiveScore.Common.Services;
using LiveScore.Core;
using LiveScore.Core.Enumerations;
using LiveScore.Core.Models.Matches;
using LiveScore.Soccer.Converters;
using LiveScore.Soccer.Models.Matches;
using NSubstitute;
using Xunit;

namespace Soccer.Tests.Converters
{
    public class MatchMinuteConverterTests
    {
        private readonly ISettings settings;
        private readonly ILoggingService loggingService;
        private readonly MatchMinuteConverter matchMinuteConverter;
        private readonly Fixture fixture;

        public MatchMinuteConverterTests()
        {
            settings = Substitute.For<ISettings>();
            loggingService = Substitute.For<ILoggingService>();
            matchMinuteConverter = new MatchMinuteConverter(settings, loggingService);

            fixture = new Fixture();
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
            var eventDate = fixture.Create<DateTime>();
            var queryTime = new DateTimeOffset(eventDate);
            byte timeShow = 35;
            
            var matchResult = new MatchResult(fixture.Create<MatchStatus>(), MatchStatus.FirstHalf);
            
            var match = new Match(eventDate, matchResult);
            var actual = matchMinuteConverter.BuildMatchMinute(match, queryTime.AddMinutes(timeShow));

            Assert.Equal($"{timeShow+1}'", actual);
        }
    }
}