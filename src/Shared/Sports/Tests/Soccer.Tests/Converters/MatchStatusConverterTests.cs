namespace Soccer.Tests.Converters
{
    using System;
    using System.Collections.Generic;
    using LiveScore.Common.LangResources;
    using LiveScore.Core.Enumerations;
    using LiveScore.Core.Models.Matches;
    using LiveScore.Soccer.Converters;
    using LiveScore.Soccer.Models.Matches;
    using Xunit;

    public class MatchStatusConverterTests
    {
        private readonly MatchStatusConverter converter;

        public MatchStatusConverterTests()
        {
            converter = new MatchStatusConverter();
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
                    EventStatus = MatchStatus.NotStartedStatus
                }
            };

            // Act
            var status = converter.BuildStatus(match);

            // Assert
            Assert.Equal("12:20", status);
        }

        [Theory]
        [InlineData(MatchStatus.Interrupted, "INT")]
        [InlineData(MatchStatus.Delayed, "Delayed")]
        [InlineData(MatchStatus.Abandoned, "AB")]
        [InlineData(MatchStatus.Pause, "Pause")]
        [InlineData(MatchStatus.Halftime, "HT")]
        [InlineData(MatchStatus.AwaitingPenalties, "Await Pen.")]
        [InlineData(MatchStatus.Penalties, "Pen.")]
        public void BuildStatus_EventStatusIsLive_TextStatus_ShowExpectedText(string matchStatus, string expectedStatus)
        {
            // Arrange
            var match = new Match
            {
                EventDate = new DateTime(2019, 01, 01, 12, 20, 00),
                MatchResult = new MatchResult
                {
                    EventStatus = MatchStatus.LiveStatus,
                    MatchStatus = new MatchStatus { Value = matchStatus }
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
                    EventStatus = MatchStatus.LiveStatus,
                    MatchTime = 30
                }
            };

            // Act
            var status = converter.BuildStatus(match);

            // Assert
            Assert.Equal("30'", status);
        }

        [Theory]
        [InlineData(MatchStatus.FirstHaft, 45, 47, 2)]
        [InlineData(MatchStatus.SecondHaft, 90, 97, 5)]
        [InlineData(MatchStatus.FirstHaftExtra, 105, 105, 1)]
        [InlineData(MatchStatus.SecondHaftExtra, 120, 125, 5)]
        [InlineData(MatchStatus.SecondHaftExtra, 120, 100, 5)]
        public void BuildStatus_EventStatusIsLive_InjuryTimeShown_ShowMatchMinuteWithInjuryTime(
            string matchStatus, int periodEndTime, int currentMatchTime, int expectedInjuryTime)
        {
            // Arrange
            var match = new Match
            {
                EventDate = new DateTime(2019, 01, 01, 12, 20, 00),
                MatchResult = new MatchResult
                {
                    EventStatus = MatchStatus.LiveStatus,
                    MatchStatus = new MatchStatus { Value = matchStatus },
                    MatchTime = currentMatchTime
                },
                LatestTimeline = new Timeline
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
        [InlineData(MatchStatus.EndedAfterPenalties, "A.P")]
        [InlineData(MatchStatus.EndedExtraTime, "A.E.T")]
        [InlineData(MatchStatus.Closed, "FT")]
        [InlineData(MatchStatus.Ended, "FT")]
        [InlineData(MatchStatus.FullTime, "FT")]
        [InlineData(MatchStatus.AwaitingExtraTime, "Await ET.")]
        public void BuildStatus_EventStatusIsClosed_ReturnExpectedStatus(string matchStatus, string expectedStatus)
        {
            // Arrange
            var match = new Match
            {
                MatchResult = new MatchResult
                {
                    EventStatus = MatchStatus.ClosedStatus,
                    MatchStatus = new MatchStatus { Value = matchStatus }
                }
            };

            // Act
            var status = converter.BuildStatus(match);

            // Assert
            Assert.Equal(expectedStatus, status);
        }

        [Theory]
        [InlineData(MatchStatus.Postponed, "Postp.")]
        [InlineData(MatchStatus.StartDelayed, "Start Delayed")]
        [InlineData(MatchStatus.Cancelled, "Canc.")]
        [InlineData(MatchStatus.AwaitingExtraTime, "Await ET.")]
        public void BuildStatus_OtherStatus_ReturnExpectedStatus(string matchStatus, string expectedStatus)
        {
            // Arrange
            var match = new Match
            {
                MatchResult = new MatchResult
                {
                    EventStatus = new MatchStatus { Value = matchStatus }
                }
            };

            // Act
            var status = converter.BuildStatus(match);

            // Assert
            Assert.Equal(expectedStatus, status);
        }
    }
}