namespace Soccer.Tests.Converters
{
    using System;
    using LiveScore.Common.LangResources;
    using LiveScore.Common.Services;
    using LiveScore.Core.Enumerations;
    using LiveScore.Core.Models.Matches;
    using LiveScore.Soccer.Converters;
    using LiveScore.Soccer.Models.Matches;
    using NSubstitute;
    using Xunit;

    public class MatchStatusConverterTests
    {
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
            var match = new Match
            {
                EventDate = new DateTime(2019, 01, 01, 12, 20, 00),
                MatchResult = new MatchResult
                {
                    EventStatus = MatchStatus.NotStarted
                }
            };

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
        public void BuildStatus_EventStatusIsLive_TextStatus_ShowExpectedText(string matchStatus, string expectedStatus)
        {
            // Arrange
            var match = new Match
            {
                EventDate = new DateTime(2019, 01, 01, 12, 20, 00),
                MatchResult = new MatchResult
                {
                    EventStatus = MatchStatus.Live,
                    MatchStatus = Enumeration.FromDisplayName<MatchStatus>(matchStatus),
                }
            };

            // Act
            var status = converter.BuildStatus(match);

            // Assert
            Assert.Equal(expectedStatus, status);
        }

        [Fact]
        public void BuildStatus_EventStatusIsLive_ShowMatchMinute()
        {
            // Arrange
            var match = new Match
            {
                EventDate = new DateTime(2019, 01, 01, 12, 20, 00),
                MatchResult = new MatchResult
                {
                    EventStatus = MatchStatus.Live,
                    MatchTime = 30
                }
            };

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
            var match = new Match
            {
                EventDate = new DateTime(2019, 01, 01, 12, 20, 00),
                MatchResult = new MatchResult
                {
                    EventStatus = MatchStatus.Live,
                    MatchStatus = Enumeration.FromDisplayName<MatchStatus>(matchStatus),
                    MatchTime = currentMatchTime
                },
                LatestTimeline = new TimelineEvent
                {
                    Type = EventTypes.InjuryTimeShown,
                    InjuryTimeAnnounced = 5
                }
            };

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
            var match = new Match
            {
                MatchResult = new MatchResult
                {
                    EventStatus = MatchStatus.Closed,
                    MatchStatus = Enumeration.FromDisplayName<MatchStatus>(matchStatus)
                }
            };

            // Act
            var status = converter.BuildStatus(match);

            // Assert
            Assert.Equal(expectedStatus, status);
        }

        [Theory]
        [InlineData("postponed", "Postp.")]
        [InlineData("start_delayed", "Start Delayed")]
        [InlineData("cancelled", "Canc.")]
        [InlineData("awaiting_extra_time", "Await ET")]
        public void BuildStatus_OtherStatus_ReturnExpectedStatus(string matchStatus, string expectedStatus)
        {
            // Arrange
            var match = new Match
            {
                MatchResult = new MatchResult
                {
                    EventStatus = Enumeration.FromDisplayName<MatchStatus>(matchStatus)
                }
            };

            // Act
            var status = converter.BuildStatus(match);

            // Assert
            Assert.Equal(expectedStatus, status);
        }

        [Fact]
        public void BuildStatus_InInjuryTimeShown_ShowExpectedStatus()
        {
            // Arrange
            var match = new Match
            {
                MatchResult = new MatchResult
                {
                    MatchStatus = MatchStatus.FirstHalfExtra,
                    EventStatus = MatchStatus.Live,
                    MatchTime = 106
                },
                LatestTimeline = new TimelineEvent
                {
                    Type = Enumeration.FromDisplayName<EventTypes>("injury_time_shown"),
                    StoppageTime = "1",
                    InjuryTimeAnnounced = 3
                }
            };

            // Act
            var status = converter.BuildStatus(match);

            // Assert
            Assert.Equal("105+1'", status);
        }

        [Theory]
        [InlineData(107, "105+2'")]
        [InlineData(110, "105+4'")]
        public void BuildStatus_InEventHasStoppageTime_ShowExpectedStatus(int matchTime, string expectedStatus)
        {
            // Arrange
            localStorage.GetValueOrDefault("InjuryTimeAnnouced123", 0).Returns(4);
            var match = new Match
            {
                Id = "123",
                MatchResult = new MatchResult
                {
                    MatchStatus = MatchStatus.FirstHalfExtra,
                    EventStatus = MatchStatus.Live,
                    MatchTime = matchTime
                },
                LatestTimeline = new TimelineEvent
                {
                    Type = Enumeration.FromDisplayName<EventTypes>("yellow_card"),
                    StoppageTime = "2",
                }
            };

            // Act
            var status = converter.BuildStatus(match);

            // Assert
            Assert.Equal(expectedStatus, status);
        }
    }
}