//namespace Soccer.Tests.Converters
//{
//    using System;
//    using LiveScore.Common.LangResources;
//    using LiveScore.Common.Services;
//    using LiveScore.Core.Enumerations;
//    using LiveScore.Core.Models.Matches;
//    using LiveScore.Soccer.Converters;
//    using LiveScore.Soccer.Models.Matches;
//    using NSubstitute;
//    using Xunit;

//    public class MatchStatusConverterTests
//    {
//        private readonly MatchStatusConverter converter;
//        private readonly ILocalStorage localStorage;

//        public MatchStatusConverterTests()
//        {
//            localStorage = Substitute.For<ILocalStorage>();
//            converter = new MatchStatusConverter(localStorage);
//        }

//        [Fact]
//        public void BuildStatus_MatchIsNull_ReturnFT()
//        {
//            // Act
//            var status = converter.BuildStatus(null);

//            // Assert
//            Assert.Equal(AppResources.FT, status);
//        }

//        [Fact]
//        public void BuildStatus_EventStatusIsNotStarted_ReturnEventTime()
//        {
//            // Arrange
//            var match = new Match
//            {
//                EventDate = new DateTime(2019, 01, 01, 12, 20, 00),
//                MatchResult = new MatchResult
//                {
//                    EventStatus = MatchStatus.NotStarted
//                }
//            };

//            // Act
//            var status = converter.BuildStatus(match);

//            // Assert
//            Assert.Equal("12:20", status);
//        }

//        //TODO update theory
//        [Theory]
//        [InlineData("interupted", "INT")]
//        //[InlineData(MatchStatus.Delayed, "Delayed")]
//        //[InlineData(MatchStatus.Abandoned, "AB")]
//        //[InlineData(MatchStatus.Pause, "Pause")]
//        //[InlineData(MatchStatus.Halftime, "HT")]
//        //[InlineData(MatchStatus.AwaitingPenalties, "Await Pen.")]
//        //[InlineData(MatchStatus.Penalties, "Pen.")]
//        //[InlineData(MatchStatus.ExtraTimeHalfTime, "ET.HT")]
//        public void BuildStatus_EventStatusIsLive_TextStatus_ShowExpectedText(string matchStatus, string expectedStatus)
//        {
//            // Arrange
//            var match = new Match
//            {
//                EventDate = new DateTime(2019, 01, 01, 12, 20, 00),
//                MatchResult = new MatchResult
//                {
//                    EventStatus = MatchStatus.Live,
//                    MatchStatus = new MatchStatus { DisplayName = matchStatus }
//                }
//            };

//            // Act
//            var status = converter.BuildStatus(match);

//            // Assert
//            Assert.Equal(expectedStatus, status);
//        }

//        [Fact]
//        public void BuildStatus_EventStatusIsLive_ShowMatchMinute()
//        {
//            // Arrange
//            var match = new Match
//            {
//                EventDate = new DateTime(2019, 01, 01, 12, 20, 00),
//                MatchResult = new MatchResult
//                {
//                    EventStatus = MatchStatus.Live,
//                    MatchTime = 30
//                }
//            };

//            // Act
//            var status = converter.BuildStatus(match);

//            // Assert
//            Assert.Equal("30'", status);
//        }

//        [Theory]
//        [InlineData("1st_half", 45, 47, 2)]
//        //[InlineData(MatchStatus.SecondHalf, 90, 97, 5)]
//        //[InlineData(MatchStatus.FirstHalfExtra, 105, 105, 1)]
//        //[InlineData(MatchStatus.SecondHalfExtra, 120, 125, 5)]
//        //[InlineData(MatchStatus.SecondHalfExtra, 120, 100, 5)]
//        public void BuildStatus_EventStatusIsLive_InjuryTimeShown_ShowMatchMinuteWithInjuryTime(
//            string matchStatus, int periodEndTime, int currentMatchTime, int expectedInjuryTime)
//        {
//            // Arrange
//            var match = new Match
//            {
//                EventDate = new DateTime(2019, 01, 01, 12, 20, 00),
//                MatchResult = new MatchResult
//                {
//                    EventStatus = MatchStatus.Live,
//                    MatchStatus = new MatchStatus { DisplayName = matchStatus },
//                    MatchTime = currentMatchTime
//                },
//                LatestTimeline = new Timeline
//                {
//                    Type = EventTypes.InjuryTimeShown,
//                    InjuryTimeAnnounced = 5
//                }
//            };

//            // Act
//            var status = converter.BuildStatus(match);

//            // Assert
//            Assert.Equal($"{periodEndTime}+{expectedInjuryTime}'", status);
//        }

//        [Theory]
//        [InlineData("ap", "AP")]
//        //[InlineData(MatchStatus.EndedExtraTime, "AET")]
//        //[InlineData(MatchStatus.Closed, "FT")]
//        //[InlineData(MatchStatus.Ended, "FT")]
//        //[InlineData(MatchStatus.FullTime, "FT")]
//        //[InlineData(MatchStatus.AwaitingExtraTime, "Await ET.")]
//        public void BuildStatus_EventStatusIsClosed_ReturnExpectedStatus(string matchStatus, string expectedStatus)
//        {
//            // Arrange
//            var match = new Match
//            {
//                MatchResult = new MatchResult
//                {
//                    EventStatus = MatchStatus.Closed,
//                    MatchStatus = new MatchStatus { DisplayName = matchStatus }
//                }
//            };

//            // Act
//            var status = converter.BuildStatus(match);

//            // Assert
//            Assert.Equal(expectedStatus, status);
//        }

//        [Theory]
//        [InlineData("postponed", "Postp.")]
//        //[InlineData(MatchStatus.StartDelayed, "Start Delayed")]
//        //[InlineData(MatchStatus.Cancelled, "Canc.")]
//        //[InlineData(MatchStatus.AwaitingExtraTime, "Await ET.")]
//        public void BuildStatus_OtherStatus_ReturnExpectedStatus(string matchStatus, string expectedStatus)
//        {
//            // Arrange
//            var match = new Match
//            {
//                MatchResult = new MatchResult
//                {
//                    EventStatus = new MatchStatus { DisplayName = matchStatus }
//                }
//            };

//            // Act
//            var status = converter.BuildStatus(match);

//            // Assert
//            Assert.Equal(expectedStatus, status);
//        }

//        [Fact]
//        public void BuildStatus_InInjuryTimeShown_ShowExpectedStatus()
//        {
//            // Arrange
//            var match = new Match
//            {
//                MatchResult = new MatchResult
//                {
//                    MatchStatus = MatchStatus.FirstHalfExtra,
//                    EventStatus = MatchStatus.Live,
//                    MatchTime = 106
//                },
//                LatestTimeline = new Timeline
//                {
//                    Type = "injury_time_shown",
//                    StoppageTime = "1",
//                    InjuryTimeAnnounced = 3
//                }
//            };

//            // Act
//            var status = converter.BuildStatus(match);

//            // Assert
//            Assert.Equal("105+1'", status);
//        }

//        [Theory]
//        [InlineData(107, "105+2'")]
//        [InlineData(110, "105+4'")]
//        public void BuildStatus_InEventHasStoppageTime_ShowExpectedStatus(int matchTime, string expectedStatus)
//        {
//            // Arrange
//            localStorage.GetValueOrDefault("InjuryTimeAnnouced123", 0).Returns(4);
//            var match = new Match
//            {
//                Id = "123",
//                MatchResult = new MatchResult
//                {
//                    MatchStatus = MatchStatus.FirstHalfExtra,
//                    EventStatus = MatchStatus.Live,
//                    MatchTime = matchTime
//                },
//                LatestTimeline = new Timeline
//                {
//                    Type = "yellow_card",
//                    StoppageTime = "2",
//                }
//            };

//            // Act
//            var status = converter.BuildStatus(match);

//            // Assert
//            Assert.Equal(expectedStatus, status);
//        }
//    }
//}