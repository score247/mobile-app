namespace Soccer.Tests.Converters
{
    using System;
    using AutoFixture;
    using LiveScore.Common.LangResources;
    using LiveScore.Common.Services;
    using LiveScore.Common.Tests.Extensions;
    using LiveScore.Core.Enumerations;
    using LiveScore.Soccer.Converters;
    using LiveScore.Soccer.Models.Matches;
    using NSubstitute;
    using Xunit;

    public class MatchStatusConverterTests
    {
        private Fixture specimens;
        private readonly MatchStatusConverter converter;
        private readonly ICachingService localStorage;

        public MatchStatusConverterTests()
        {
            localStorage = Substitute.For<ICachingService>();
            converter = new MatchStatusConverter(localStorage);
        }

        [Fact]
        public void BuildStatus_MatchIsNull_ReturnFT()
        {
            // Act
            var status = converter.BuildStatus(null);

            // Assert
            Assert.Equal(AppResources.FT, status);
        }

        [Fact]
        public void BuildStatus_EventStatusIsNotStarted_ReturnEventTime()
        {
            // Arrange
            specimens = new Fixture();
            var match = specimens.For<Match>()
                .With(x => x.EventDate, new DateTime(2019, 01, 01, 12, 20, 00))
                .With(x => x.EventStatus, MatchStatus.NotStarted)
                .Create();

            // Act
            var status = converter.BuildStatus(match);

            // Assert
            Assert.Equal("12:20", status);
        }

        [Theory]
        [InlineData("interrupted", "INT")]
        [InlineData("delayed", "Delayed")]
        [InlineData("abandoned", "AB")]
        [InlineData("pause", "Pause")]
        [InlineData("halftime", "HT")]
        [InlineData("awaiting_penalties", "Await Pen")]
        [InlineData("penalties", "Pen")]
        [InlineData("extra_time_halftime", "ETHT")]
        public void BuildStatus_EventStatusIsLive_TextStatus_ShowExpectedText(string matchStatusValue, string expectedStatus)
        {
            // Arrange
            specimens = new Fixture();
            var matchStatus = Enumeration.FromDisplayName<MatchStatus>(matchStatusValue);

            var match = specimens.For<Match>()
                .With(x => x.EventStatus, MatchStatus.Live)
                .With(x => x.MatchStatus, matchStatus)
                .Create();

            // Act
            var status = converter.BuildStatus(match);

            // Assert
            Assert.Equal(expectedStatus, status);
        }

        [Fact]
        public void BuildStatus_EventStatusIsLive_ShowMatchMinute()
        {
            // Arrange
            specimens = new Fixture();
            var match = specimens.For<Match>()
                  .With(x => x.EventStatus, MatchStatus.Live)
                  .With(x => x.MatchTime, (byte)30)
                  .With(x => x.StoppageTime, null)
                  .Create();

            // Act
            var status = converter.BuildStatus(match);

            // Assert
            Assert.Equal("30'", status);
        }

        [Theory]
        [InlineData("1st_half", 45, 47, 2)]
        [InlineData("2nd_half", 90, 97, 5)]
        [InlineData("1st_extra", 105, 105, 1)]
        [InlineData("2nd_extra", 120, 125, 5)]
        [InlineData("2nd_extra", 120, 100, 5)]
        public void BuildStatus_EventStatusIsLive_InjuryTimeShown_ShowMatchMinuteWithInjuryTime(
                string matchStatus, int periodEndTime, int currentMatchTime, int expectedInjuryTime)
        {
            // Arrange
            specimens = new Fixture();
            var match = specimens.For<Match>()
                  .With(x => x.EventStatus, MatchStatus.Live)
                  .With(x => x.MatchStatus, Enumeration.FromDisplayName<MatchStatus>(matchStatus))
                  .With(x => x.MatchTime, (byte)currentMatchTime)
                  .With(x => x.LastTimelineType, EventType.InjuryTimeShown)
                  .With(x => x.InjuryTimeAnnounced, (byte)5)
                  .Create();

            // Act
            var status = converter.BuildStatus(match);

            // Assert
            Assert.Equal($"{periodEndTime}+{expectedInjuryTime}'", status);
        }

        [Theory]
        [InlineData("ap", "AP")]
        [InlineData("aet", "AET")]
        [InlineData("closed", "FT")]
        [InlineData("ended", "FT")]
        [InlineData("full-time", "FT")]
        [InlineData("awaiting_extra_time", "Await ET")]
        public void BuildStatus_EventStatusIsClosed_ReturnExpectedStatus(string matchStatus, string expectedStatus)
        {
            // Arrange
            specimens = new Fixture();
            var match = specimens.For<Match>()
                  .With(x => x.EventStatus, MatchStatus.Closed)
                  .With(x => x.MatchStatus, Enumeration.FromDisplayName<MatchStatus>(matchStatus))
                  .Create();

            // Act
            var status = converter.BuildStatus(match);

            // Assert
            Assert.Equal(expectedStatus, status);
        }

        [Theory]
        [InlineData("postponed", "Postp")]
        [InlineData("start_delayed", "Start Delayed")]
        [InlineData("cancelled", "Canc")]
        [InlineData("awaiting_extra_time", "Await ET")]
        public void BuildStatus_OtherStatus_ReturnExpectedStatus(string matchStatus, string expectedStatus)
        {
            // Arrange
            specimens = new Fixture();
            var match = specimens.For<Match>()
                  .With(x => x.EventStatus, Enumeration.FromDisplayName<MatchStatus>(matchStatus))
                  .Create();

            // Act
            var status = converter.BuildStatus(match);

            // Assert
            Assert.Equal(expectedStatus, status);
        }

        [Fact]
        public void BuildStatus_InInjuryTimeShown_ShowExpectedStatus()
        {
            // Arrange
            specimens = new Fixture();
            var match = specimens.For<Match>()
                  .With(x => x.EventStatus, MatchStatus.Live)
                  .With(x => x.MatchStatus, MatchStatus.FirstHalfExtra)
                  .With(x => x.MatchTime, (byte)106)
                  .With(x => x.LastTimelineType, EventType.InjuryTimeShown)
                  .With(x => x.StoppageTime, "1")
                  .With(x => x.InjuryTimeAnnounced, (byte)3)
                  .Create();

            // Act
            var status = converter.BuildStatus(match);

            // Assert
            Assert.Equal("105+1'", status);
        }

        [Theory]
        [InlineData(107, "105+2'")]
        [InlineData(110, "105+4'")]
        public void BuildStatus_InEventHasStoppageTime_ShowExpectedStatus(byte matchTime, string expectedStatus)
        {
            // Arrange
            localStorage.GetValueOrDefaultInMemory("InjuryTimeAnnouced123", 0).Returns(4);
            specimens = new Fixture();
            var match = specimens.For<Match>()
                  .With(x => x.Id, "123")
                  .With(x => x.EventStatus, MatchStatus.Live)
                  .With(x => x.MatchStatus, MatchStatus.FirstHalfExtra)
                  .With(x => x.MatchTime, matchTime)
                  .With(x => x.LastTimelineType, EventType.YellowCard)
                  .With(x => x.StoppageTime, "2")
                  .With(x => x.InjuryTimeAnnounced, (byte)0)
                  .Create();

            // Act
            var status = converter.BuildStatus(match);

            // Assert
            Assert.Equal(expectedStatus, status);
        }
    }
}