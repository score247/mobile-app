using System.Collections.Generic;
using AutoFixture;
using LiveScore.Core.Enumerations;
using LiveScore.Core.Models.Matches;
using LiveScore.Core.Tests.Fixtures;
using LiveScore.Soccer.Models.Matches;
using LiveScore.Soccer.ViewModels.MatchDetails.Information.InfoItems;
using Xamarin.Forms;
using Xunit;

namespace Soccer.Tests.ViewModels.MatchDetailInfo
{
    public class MainEventItemViewModelTests : IClassFixture<ViewModelBaseFixture>, IClassFixture<ResourcesFixture>
    {
        private readonly ViewModelBaseFixture baseFixture;
        private readonly Fixture fixture;

        public MainEventItemViewModelTests(ViewModelBaseFixture baseFixture)
        {
            this.baseFixture = baseFixture;
            fixture = baseFixture.CommonFixture.Specimens;
        }

        [Fact]
        public void BuildInfo_Always_SetRowColor()
        {
            // Arrange
            var matchInfo = fixture.Create<MatchInfo>();
            var timeline = fixture.Create<TimelineEvent>();
            var viewModel = new MainEventItemViewModel(timeline, matchInfo, baseFixture.NavigationService, baseFixture.DependencyResolver);

            // Act
            viewModel.BuildData();

            // Assert
            Assert.Equal(Color.FromHex("#1D2133"), viewModel.RowColor);
        }

        [Fact]
        public void BuildInfo_NoMatchPeriodResult_ShowHyphen()
        {
            // Arrange
            var matchInfo = fixture.Create<MatchInfo>();
            var timeline = fixture.Create<TimelineEvent>();
            var viewModel = new MainEventItemViewModel(timeline, matchInfo, baseFixture.NavigationService, baseFixture.DependencyResolver);

            // Act
            viewModel.BuildData();

            // Assert
            Assert.Equal("-", viewModel.Score);
        }

        [Fact]
        public void BuildMainEventStatus_BreakStart_PausePeriod_ShowHalfTime()
        {
            // Arrange
            var matchInfo = fixture.Create<MatchInfo>();
            var timeline = fixture.Create<TimelineEvent>()
                .With(timeline => timeline.PeriodType, PeriodType.Pause)
                .With(timeline => timeline.Type, EventType.BreakStart)
                .With(timeline => timeline.HomeScore, (byte)1)
                .With(timeline => timeline.AwayScore, (byte)2);
            var viewModel = new MainEventItemViewModel(timeline, matchInfo, baseFixture.NavigationService, baseFixture.DependencyResolver);
           
            // Act
            viewModel.BuildData();

            // Assert
            Assert.Equal("Half Time", viewModel.MainEventStatus);
            Assert.Equal("1 - 2", viewModel.Score);
        }

        [Fact]
        public void BuildMainEventStatus_BreakStart_ExtraTimeHalfTime_ShowHalfTime()
        {
            // Arrange
            var matchInfo = fixture.Create<MatchInfo>();
            var timeline = fixture.Create<TimelineEvent>()
                .With(timeline => timeline.PeriodType, PeriodType.ExtraTimeHalfTime)
                .With(timeline => timeline.Type, EventType.BreakStart);
            var viewModel = new MainEventItemViewModel(timeline, matchInfo, baseFixture.NavigationService, baseFixture.DependencyResolver);

            // Act
            viewModel.BuildData();

            // Assert
            Assert.Equal("Extra Time Half Time", viewModel.MainEventStatus);
            Assert.Equal($"{timeline.HomeScore} - {timeline.AwayScore}", viewModel.Score);
        }

        [Fact]
        public void BuildMainEventStatus_MatchEnd_MatchStatusIsEnded_ShowFullTime()
        {
            // Arrange
            var matchInfo = fixture.Create<MatchInfo>()
                .With(match => match.Match, new SoccerMatch(new MatchResult(MatchStatus.Closed, MatchStatus.Ended)));

            var timeline = fixture.Create<TimelineEvent>()
                .With(timeline => timeline.Type, EventType.MatchEnded)
                .With(timeline => timeline.HomeScore, (byte)3)
                .With(timeline => timeline.AwayScore, (byte)5);
            var viewModel = new MainEventItemViewModel(timeline, matchInfo, baseFixture.NavigationService, baseFixture.DependencyResolver);

            // Act
            viewModel.BuildData();

            // Assert
            Assert.Equal("Full Time", viewModel.MainEventStatus);
            Assert.Equal("3 - 5", viewModel.Score);
        }

        [Fact]
        public void BuildMainEventStatus_BreakStart_PeriodIsAwaitExtra_ShowFullTime()
        {
            // Arrange
            var matchInfo = fixture.Create<MatchInfo>();

            var timeline = fixture.Create<TimelineEvent>()
                .With(timeline => timeline.Type, EventType.BreakStart)
                .With(timeline => timeline.PeriodType, PeriodType.AwaitingExtraTime)
                .With(timeline => timeline.HomeScore, (byte)3)
                .With(timeline => timeline.AwayScore, (byte)5);
            var viewModel = new MainEventItemViewModel(timeline, matchInfo, baseFixture.NavigationService, baseFixture.DependencyResolver);

            // Act
            viewModel.BuildData();

            // Assert
            Assert.Equal("Full Time", viewModel.MainEventStatus);
            Assert.Equal("3 - 5", viewModel.Score);
        }

        [Fact]
        public void BuildMainEventStatus_BreakStart_PeriodIsAwaitPenalty_NoOvertime_ShowFullTime()
        {
            // Arrange
            var soccerMatch = fixture.Create<SoccerMatch>()
                       .With(match => match.MatchPeriods, new List<MatchPeriod>());

            var matchInfo = fixture.Create<MatchInfo>()
                .With(match => match.Match, soccerMatch);

            var timeline = fixture.Create<TimelineEvent>()
                .With(timeline => timeline.Type, EventType.BreakStart)
                .With(timeline => timeline.PeriodType, PeriodType.AwaitingPenalties)
                .With(timeline => timeline.HomeScore, (byte)3)
                .With(timeline => timeline.AwayScore, (byte)5);
            var viewModel = new MainEventItemViewModel(timeline, matchInfo, baseFixture.NavigationService, baseFixture.DependencyResolver);

            // Act
            viewModel.BuildData();

            // Assert
            Assert.Equal("Full Time", viewModel.MainEventStatus);
            Assert.Equal("3 - 5", viewModel.Score);
        }

        [Fact]
        public void BuildMainEventStatus_MatchEnd_MatchStatusIsAET_ShowAfterExtraTime()
        {
            // Arrange
            var matchInfo = fixture.Create<MatchInfo>()
                .With(match => match.Match, new SoccerMatch(new MatchResult(MatchStatus.Closed, MatchStatus.EndedExtraTime)));

            var timeline = fixture.Create<TimelineEvent>()
               .With(timeline => timeline.Type, EventType.MatchEnded)
               .With(timeline => timeline.HomeScore, (byte)3)
               .With(timeline => timeline.AwayScore, (byte)6);
            var viewModel = new MainEventItemViewModel(timeline, matchInfo, baseFixture.NavigationService, baseFixture.DependencyResolver);

            // Act
            viewModel.BuildData();

            // Assert
            Assert.Equal("After Extra Time", viewModel.MainEventStatus);
            Assert.Equal($"{timeline.HomeScore} - {timeline.AwayScore}", viewModel.Score);
        }

        [Fact]
        public void BuildMainEventStatus_BreakStart_PeriodIsAwaitPenalties_ShowAfterExtraTime()
        {
            // Arrange
            var soccerMatch = fixture.Create<SoccerMatch>()
                       .With(match => match.MatchPeriods, new List<MatchPeriod> {
                new MatchPeriod { HomeScore = 1, AwayScore = 2 },
                new MatchPeriod { HomeScore = 2, AwayScore = 3 },
                new MatchPeriod { HomeScore = 2, AwayScore = 3, PeriodType = PeriodType.Overtime }
            });

            var matchInfo = fixture.Create<MatchInfo>()
                .With(match => match.Match, soccerMatch);

            var timeline = fixture.Create<TimelineEvent>()
                .With(timeline => timeline.Type, EventType.BreakStart)
                .With(timeline => timeline.PeriodType, PeriodType.AwaitingPenalties);

            var viewModel = new MainEventItemViewModel(timeline, matchInfo, baseFixture.NavigationService, baseFixture.DependencyResolver);

            // Act
            viewModel.BuildData();

            // Assert
            Assert.Equal("After Extra Time", viewModel.MainEventStatus);
            Assert.Equal($"{timeline.HomeScore} - {timeline.AwayScore}", viewModel.Score);
        }

        [Fact]
        public void BuildMainEventStatus_MatchEndAfterPenalties_ShowPenaltyShootOut()
        {
            // Arrange
            var soccerMatch = fixture.Create<SoccerMatch>()
                       .With(match => match.MatchPeriods, new List<MatchPeriod> 
                       {
                            new MatchPeriod { HomeScore = 1, AwayScore = 2 },
                            new MatchPeriod { HomeScore = 2, AwayScore = 3 },
                            new MatchPeriod { HomeScore = 0, AwayScore = 2, PeriodType = PeriodType.Overtime},
                            new MatchPeriod { HomeScore = 4, AwayScore = 3, PeriodType = PeriodType.Penalties }
                        })
                       .With(match => match.MatchStatus, MatchStatus.EndedAfterPenalties);

            var matchInfo = fixture.Create<MatchInfo>()
                .With(match => match.Match, soccerMatch);

            var timeline = fixture.Create<TimelineEvent>()
                .With(timeline => timeline.Type, EventType.MatchEnded)
                .With(timeline => timeline.PeriodType, PeriodType.Penalties);

            var viewModel = new MainEventItemViewModel(timeline, matchInfo, baseFixture.NavigationService, baseFixture.DependencyResolver);

            // Act
            viewModel.BuildData();

            // Assert
            Assert.Equal("Penalty Shoot-Out", viewModel.MainEventStatus);
            Assert.Equal("4 - 3", viewModel.Score);
        }
    }
}