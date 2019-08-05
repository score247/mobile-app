//namespace Soccer.Tests.ViewModels.MatchDetailInfo
//{
//    using System.Collections.Generic;
//    using LiveScore.Core.Enumerations;
//    using LiveScore.Core.Models.Matches;
//    using LiveScore.Core.Tests.Fixtures;
//    using LiveScore.Soccer.ViewModels.MatchDetailInfo;
//    using NSubstitute;
//    using Xamarin.Forms;
//    using Xunit;

//    public class MainEventItemViewModelTests : IClassFixture<ViewModelBaseFixture>, IClassFixture<ResourcesFixture>
//    {
//        private readonly ITimeline timeline;
//        private readonly IMatchResult matchResult;
//        private readonly ViewModelBaseFixture baseFixture;

//        public MainEventItemViewModelTests(ViewModelBaseFixture baseFixture)
//        {
//            this.baseFixture = baseFixture;
//            timeline = Substitute.For<ITimeline>();
//            matchResult = Substitute.For<IMatchResult>();
//        }

//        [Fact]
//        public void BuildInfo_Always_SetRowColor()
//        {
//            // Act
//            var viewModel = new MainEventItemViewModel(timeline, null, baseFixture.NavigationService, baseFixture.DependencyResolver);

//            // Assert
//            Assert.Equal(Color.FromHex("#1D2133"), viewModel.RowColor);
//        }

//        [Fact]
//        public void BuildInfo_NoMatchPeriodResult_ShowHyphen()
//        {
//            // Act
//            var viewModel = new MainEventItemViewModel(timeline, null, baseFixture.NavigationService, baseFixture.DependencyResolver);

//            // Assert
//            Assert.Equal("-", viewModel.Score);
//        }

//        [Fact]
//        public void BuildMainEventStatus_BreakStart_PausePeriod_ShowHalfTime()
//        {
//            // Arrange
//            timeline.Type.Returns("break_start");
//            timeline.PeriodType.Returns("pause");
//            matchResult.MatchPeriods.Returns(new List<MatchPeriod> {
//                new MatchPeriod { HomeScore = 1, AwayScore = 2 }
//            });

//            // Act
//            var viewModel = new MainEventItemViewModel(timeline, matchResult, baseFixture.NavigationService, baseFixture.DependencyResolver);

//            // Assert
//            Assert.Equal("Half Time", viewModel.MainEventStatus);
//            Assert.Equal("1 - 2", viewModel.Score);
//        }

//        [Fact]
//        public void BuildMainEventStatus_MatchEnd_MatchStatusIsEnded_ShowFullTime()
//        {
//            // Arrange
//            timeline.Type.Returns("match_ended");
//            matchResult.MatchStatus.Returns(new MatchStatus { DisplayName = "ended" });
//            matchResult.MatchPeriods.Returns(new List<MatchPeriod> {
//                new MatchPeriod { HomeScore = 1, AwayScore = 2 },
//                new MatchPeriod { HomeScore = 2, AwayScore = 3 }
//            });

//            // Act
//            var viewModel = new MainEventItemViewModel(timeline, matchResult, baseFixture.NavigationService, baseFixture.DependencyResolver);

//            // Assert
//            Assert.Equal("Full Time", viewModel.MainEventStatus);
//            Assert.Equal("3 - 5", viewModel.Score);
//        }

//        [Fact]
//        public void BuildMainEventStatus_BreakStart_PeriodIsAwaitExtra_ShowFullTime()
//        {
//            // Arrange
//            timeline.Type.Returns("break_start");
//            timeline.PeriodType.Returns("awaiting_extra");
//            matchResult.MatchPeriods.Returns(new List<MatchPeriod> {
//                new MatchPeriod { HomeScore = 1, AwayScore = 2 },
//                new MatchPeriod { HomeScore = 2, AwayScore = 3 },
//                new MatchPeriod { HomeScore = 2, AwayScore = 3, PeriodType = PeriodTypes.Overtime }
//            });

//            // Act
//            var viewModel = new MainEventItemViewModel(timeline, matchResult, baseFixture.NavigationService, baseFixture.DependencyResolver);

//            // Assert
//            Assert.Equal("Full Time", viewModel.MainEventStatus);
//            Assert.Equal("3 - 5", viewModel.Score);
//        }

//        [Fact]
//        public void BuildMainEventStatus_BreakStart_PeriodIsAwaitPenalty_NoOvertime_ShowFullTime()
//        {
//            // Arrange
//            timeline.Type.Returns("break_start");
//            timeline.PeriodType.Returns("awaiting_penalties");
//            matchResult.MatchPeriods.Returns(new List<MatchPeriod> {
//                new MatchPeriod { HomeScore = 1, AwayScore = 2 },
//                new MatchPeriod { HomeScore = 2, AwayScore = 3 }
//            });

//            // Act
//            var viewModel = new MainEventItemViewModel(timeline, matchResult, baseFixture.NavigationService, baseFixture.DependencyResolver);

//            // Assert
//            Assert.Equal("Full Time", viewModel.MainEventStatus);
//            Assert.Equal("3 - 5", viewModel.Score);
//        }

//        [Fact]
//        public void BuildMainEventStatus_MatchEnd_MatchStatusIsAET_ShowAfterExtraTime()
//        {
//            // Arrange
//            timeline.Type.Returns("match_ended");
//            matchResult.MatchStatus.Returns(new MatchStatus { DisplayName = "aet" });
//            matchResult.HomeScore.Returns(3);
//            matchResult.AwayScore.Returns(6);

//            // Act
//            var viewModel = new MainEventItemViewModel(timeline, matchResult, baseFixture.NavigationService, baseFixture.DependencyResolver);

//            // Assert
//            Assert.Equal("After Extra Time", viewModel.MainEventStatus);
//            Assert.Equal("3 - 6", viewModel.Score);
//        }

//        [Fact]
//        public void BuildMainEventStatus_BreakStart_PeriodIsAwaitPenalties_ShowAfterExtraTime()
//        {
//            // Arrange
//            timeline.Type.Returns("break_start");
//            timeline.PeriodType.Returns("awaiting_penalties");
//            matchResult.HomeScore.Returns(3);
//            matchResult.AwayScore.Returns(6);
//            matchResult.MatchPeriods.Returns(new List<MatchPeriod> {
//                new MatchPeriod { HomeScore = 1, AwayScore = 2 },
//                new MatchPeriod { HomeScore = 2, AwayScore = 3 },
//                new MatchPeriod { HomeScore = 2, AwayScore = 3, PeriodType = PeriodTypes.Overtime }
//            });

//            // Act
//            var viewModel = new MainEventItemViewModel(timeline, matchResult, baseFixture.NavigationService, baseFixture.DependencyResolver);

//            // Assert
//            Assert.Equal("After Extra Time", viewModel.MainEventStatus);
//            Assert.Equal("3 - 6", viewModel.Score);
//        }

//        [Fact]
//        public void BuildMainEventStatus_PeriodStart_Penalties_ShowPenaltyShootOut()
//        {
//            // Arrange
//            //timeline.Type.Returns("period_start");
//            //timeline.PeriodType.Returns("penalties");
//            //matchResult.MatchPeriods.Returns(new List<MatchPeriod> {
//            //    new MatchPeriod { HomeScore = 1, AwayScore = 2 },
//            //    new MatchPeriod { HomeScore = 2, AwayScore = 3 },
//            //    new MatchPeriod { HomeScore = 0, AwayScore = 2, PeriodType = new PeriodTypes { DisplayName = "overtime" } },
//            //    new MatchPeriod { HomeScore = 4, AwayScore = 3, PeriodType = new PeriodTypes { DisplayName = "penalties" } }
//            //});

//            //// Act
//            //var viewModel = new MainEventItemViewModel(timeline, matchResult, baseFixture.NavigationService, baseFixture.DependencyResolver);

//            //// Assert
//            //Assert.Equal("Penalty Shoot-Out", viewModel.MainEventStatus);
//            //Assert.Equal("4 - 3", viewModel.Score);
//        }
//    }
//}