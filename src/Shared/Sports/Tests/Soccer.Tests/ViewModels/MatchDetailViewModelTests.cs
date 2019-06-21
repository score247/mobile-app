namespace Soccer.Tests.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using KellermanSoftware.CompareNetObjects;
    using LiveScore.Core.Converters;
    using LiveScore.Core.Enumerations;
    using LiveScore.Core.Models.Leagues;
    using LiveScore.Core.Models.Matches;
    using LiveScore.Core.Models.Teams;
    using LiveScore.Core.Services;
    using LiveScore.Core.Tests.Fixtures;
    using LiveScore.Soccer.Converters;
    using LiveScore.Soccer.Models.Matches;
    using LiveScore.Soccer.ViewModels;
    using LiveScore.Soccer.ViewModels.MatchDetailInfo;
    using NSubstitute;
    using Prism.Navigation;
    using Xunit;

    public class MatchDetailViewModelTests : IClassFixture<ViewModelBaseFixture>
    {
        private readonly MatchDetailViewModel viewModel;
        private readonly IMatchService matchService;
        private readonly CompareLogic comparer;
        private readonly Match match;

        public MatchDetailViewModelTests(ViewModelBaseFixture baseFixture)
        {
            comparer = baseFixture.CommonFixture.Comparer;
            baseFixture.DependencyResolver.Resolve<IMatchStatusConverter>(
                baseFixture.AppSettingsFixture.SettingsService.CurrentSportType.Value)
                .Returns(new MatchStatusConverter());
            matchService = Substitute.For<IMatchService>();
            baseFixture.DependencyResolver.Resolve<IMatchService>("1").Returns(matchService);
            viewModel = new MatchDetailViewModel(
                baseFixture.NavigationService,
                baseFixture.DependencyResolver,
                baseFixture.EventAggregator,
                baseFixture.HubService);

            match = CreateMatch();
            var parameters = new NavigationParameters { { "Match", match } };
            viewModel.OnNavigatingTo(parameters);
        }

        private Match CreateMatch()
        {
            var matchData = new Match
            {
                Id = "1234",
                League = new League { Name = "Laliga" },
                EventDate = new DateTime(2019, 01, 01, 18, 00, 00),
                Attendance = 2034,
                Venue = new Venue { Name = "My Dinh" },
                MatchResult = new MatchResult
                {
                    HomeScore = 5,
                    AwayScore = 1,
                    EventStatus = MatchStatus.LiveStatus,
                    MatchStatus = MatchStatus.FirstHaftStatus
                },
                Teams = new List<Team>
            {
                new Team { Id = "home", Name = "Barcelona" },
                new Team { Id = "away", Name = "Real Marid"}
            }
            };

            return matchData;
        }

        [Fact]
        public void OnNavigatingTo_ParametersIsNotNull_BuildMatchStatus()
        {
            // Arrange
            match.LatestTimeline = new Timeline { StoppageTime = "4", InjuryTimeAnnounced = 5 };
            match.MatchResult.MatchTime = 49;
            var parameters = new NavigationParameters { { "Match", match } };

            // Act
            viewModel.OnNavigatingTo(parameters);

            // Assert
            Assert.Equal("45+4'", viewModel.MatchViewModel.DisplayMatchStatus);
        }

        [Fact]
        public void OnNavigatingTo_ParametersIsNotNull_BuildLeagueNameAndEventDate()
        {
            // Arrange
            var parameters = new NavigationParameters { { "Match", match } };

            // Act
            viewModel.OnNavigatingTo(parameters);

            // Assert
            Assert.Equal("01 Jan, 2019 - LALIGA", viewModel.DisplayEventDateAndLeagueName);
        }

        [Fact]
        public void OnNavigatingTo_IsNotPreMatch_ShowScore()
        {
            // Arrange
            var parameters = new NavigationParameters { { "Match", match } };

            // Act
            viewModel.OnNavigatingTo(parameters);

            // Assert
            Assert.Equal("5 - 1", viewModel.DisplayScore);
        }

        [Fact]
        public void OnNavigatingTo_IsPreMatch_NotShowScore()
        {
            // Arrange
            match.MatchResult = new MatchResult
            {
                EventStatus = new MatchStatus { Value = MatchStatus.NotStarted }
            };
            var parameters = new NavigationParameters { { "Match", match } };

            // Act
            viewModel.OnNavigatingTo(parameters);

            // Assert
            Assert.Equal(" - ", viewModel.DisplayScore);
        }

        [Fact]
        public void OnNavigatingTo_HasSecondLeg_ShowSecondLegMessage()
        {
            // Arrange
            match.MatchResult = new MatchResult
            {
                AggregateHomeScore = 2,
                AggregateAwayScore = 5,
                AggregateWinnerId = "home"
            };
            var parameters = new NavigationParameters { { "Match", match } };

            // Act
            viewModel.OnNavigatingTo(parameters);

            // Assert
            Assert.Equal("2nd Leg, Aggregate Score: 2 - 5", viewModel.DisplaySecondLeg);
        }

        [Fact]
        public void OnNavigatingTo_HasSecondLeg_HomeTeamWin_ShowSecondLegImageInHome()
        {
            // Arrange
            match.MatchResult = new MatchResult
            {
                AggregateWinnerId = "home",
                WinnerId = "home"
            };
            var parameters = new NavigationParameters { { "Match", match } };

            // Act
            viewModel.OnNavigatingTo(parameters);

            // Assert
            Assert.Equal("images/common/second_leg_winner.png", match.HomeSecondLegImage);
        }

        [Fact]
        public void OnNavigatingTo_HasSecondLeg_AwayTeamWin_ShowSecondLegImageInAway()
        {
            // Arrange
            match.MatchResult = new MatchResult
            {
                AggregateWinnerId = "home",
                WinnerId = "away"
            };
            var parameters = new NavigationParameters { { "Match", match } };

            // Act
            viewModel.OnNavigatingTo(parameters);

            // Assert
            Assert.Equal("images/common/second_leg_winner.png", match.AwaySecondLegImage);
        }

        [Fact]
        public void OnNavigatingTo_HasPenalty_ShowPenaltyMessage()
        {
            // Arrange
            match.MatchResult = new MatchResult
            {
                MatchPeriods = new List<MatchPeriod> {
                    new MatchPeriod { HomeScore = 3, AwayScore = 4, PeriodType = PeriodTypes.PenaltiesType }
                }
            };
            var parameters = new NavigationParameters { { "Match", match } };

            // Act
            viewModel.OnNavigatingTo(parameters);

            // Assert
            Assert.Equal("Penalty Shoot-Out: 3 - 4", viewModel.DisplayPenaltyShootOut);
        }

        [Fact]
        public void OnNavigatingTo_HasPenalty_WinnerIsHome_ShowPenaltyImage()
        {
            // Arrange
            match.MatchResult = new MatchResult
            {
                WinnerId = "home",
                MatchPeriods = new List<MatchPeriod>
                {
                    new MatchPeriod { HomeScore = 3, AwayScore = 4, PeriodType = PeriodTypes.PenaltiesType }
                }
            };
            var parameters = new NavigationParameters { { "Match", match } };

            // Act
            viewModel.OnNavigatingTo(parameters);

            // Assert
            Assert.Equal("images/common/penalty_winner.png", match.HomePenaltyImage);
        }

        [Fact]
        public void OnNavigatingTo_HasPenalty_WinnerIsAway_ShowPenaltyImage()
        {
            // Arrange
            match.MatchResult = new MatchResult
            {
                WinnerId = "away",
                MatchPeriods = new List<MatchPeriod>
                {
                    new MatchPeriod { HomeScore = 3, AwayScore = 4, PeriodType = PeriodTypes.PenaltiesType }
                }
            };
            var parameters = new NavigationParameters { { "Match", match } };

            // Act
            viewModel.OnNavigatingTo(parameters);

            // Assert
            Assert.Equal("images/common/penalty_winner.png", match.AwayPenaltyImage);
        }

        [Fact]
        public void OnAppearing_Always_LoadMatchDetail()
        {
            // Arrange
            var returnMatch = CreateMatch();
            var returnTimelines = new List<ITimeline>
            {
                new Timeline { Type = "red_card", Time = new DateTime(2019, 01, 01, 18, 00, 00) },
                new Timeline { Type = "yellow_card", Time = new DateTime(2019, 01, 01, 17, 00, 00 )},
                new Timeline { Type = "corner_kick", Time = new DateTime(2019, 01, 01, 17, 15, 00 )},
                new Timeline { Type = "red_card", Time = new DateTime(2019, 01, 01, 17, 45, 00 )},
                new Timeline { Type = "break_start", PeriodType = "pause", Time = new DateTime(2019, 01, 01, 17, 50, 00 )},
                new Timeline { Type = "penalty_missed", Time = new DateTime(2019, 01, 01, 18, 30, 00 )},
                new Timeline { Type = "score_change", Time = new DateTime(2019, 01, 01, 17, 55, 00 )},
                new Timeline { Type = "break_start", PeriodType = "extra_time_halftime", Time = new DateTime(2019, 01, 01, 18, 40, 00 )},
                new Timeline { Type = "period_start", PeriodType = "penalties", Time = new DateTime(2019, 01, 01, 18, 55, 00 )},
                new Timeline { Type = "penalty_shootout", Time = new DateTime(2019, 01, 01, 19, 00, 00 )},
                new Timeline { Type = "match_ended", Time = new DateTime(2019, 01, 01, 19, 50, 00 )},
            };
            returnMatch.TimeLines = returnTimelines;
            returnMatch.MatchResult = new MatchResult { MatchStatus = MatchStatus.EndedAfterPenaltiesStatus };
            matchService.GetMatch(viewModel.SettingsService.UserSettings, match.Id, false).Returns(returnMatch);

            // Act
            viewModel.OnAppearing();

            // Assert
            var expectedInfoItemViewModels = new ObservableCollection<BaseItemViewModel>
            {
                new BaseItemViewModel(returnTimelines[1], returnMatch.MatchResult, viewModel.NavigationService, viewModel.DependencyResolver).CreateInstance(),
                new BaseItemViewModel(returnTimelines[3], returnMatch.MatchResult, viewModel.NavigationService, viewModel.DependencyResolver).CreateInstance(),
                new BaseItemViewModel(returnTimelines[4], returnMatch.MatchResult, viewModel.NavigationService, viewModel.DependencyResolver).CreateInstance(),
                new BaseItemViewModel(returnTimelines[6], returnMatch.MatchResult, viewModel.NavigationService, viewModel.DependencyResolver).CreateInstance(),
                new BaseItemViewModel(returnTimelines[0], returnMatch.MatchResult, viewModel.NavigationService, viewModel.DependencyResolver).CreateInstance(),
                new BaseItemViewModel(returnTimelines[5], returnMatch.MatchResult, viewModel.NavigationService, viewModel.DependencyResolver).CreateInstance(),
                new BaseItemViewModel(returnTimelines[8], returnMatch.MatchResult, viewModel.NavigationService, viewModel.DependencyResolver).CreateInstance(),
                new BaseItemViewModel(returnTimelines[9], returnMatch.MatchResult, viewModel.NavigationService, viewModel.DependencyResolver).CreateInstance()
            };
            var actualInfoItemViewModels = viewModel.InfoItemViewModels;
            Assert.True(comparer.Compare(expectedInfoItemViewModels, actualInfoItemViewModels).AreEqual);
        }

        [Fact]
        public void OnAppearing_MatchNotPenalty_LoadMatchDetail()
        {
            // Arrange
            var returnMatch = CreateMatch();
            var returnTimelines = new List<ITimeline>
            {
                new Timeline { Type = "match_ended", Time = new DateTime(2019, 01, 01, 19, 50, 00 )},
            };
            returnMatch.TimeLines = returnTimelines;
            returnMatch.MatchResult = new MatchResult { MatchStatus = MatchStatus.EndedExtraTimeStatus };
            matchService.GetMatch(viewModel.SettingsService.UserSettings, match.Id, false).Returns(returnMatch);

            // Act
            viewModel.OnAppearing();

            // Assert
            var expectedInfoItemViewModels = new ObservableCollection<BaseItemViewModel>
            {
                new BaseItemViewModel(returnTimelines[0], returnMatch.MatchResult, viewModel.NavigationService, viewModel.DependencyResolver).CreateInstance()
            };

            var actualInfoItemViewModels = viewModel.InfoItemViewModels;
            Assert.True(comparer.Compare(expectedInfoItemViewModels, actualInfoItemViewModels).AreEqual);
        }

        [Fact]
        public void OnAppearing_Always_LoadFooterInfo()
        {
            // Arrange
            matchService.GetMatch(viewModel.SettingsService.UserSettings, match.Id, false).Returns(match);

            // Act
            viewModel.OnAppearing();

            // Assert
            Assert.Equal("18:00 01 Jan, 2019", viewModel.DisplayEventDate);
            Assert.Equal("2,034", viewModel.DisplayAttendance);
            Assert.Equal("My Dinh", viewModel.DisplayVenue);
        }

        [Fact]
        public void OnReceivingMatchEvent_IsCurrentMatch_BuildGeneralInfo()
        {
            // Arrange
            match.TimeLines = new List<ITimeline>
            {
                new Timeline { Type = "yellow_card", Time = new DateTime(2019, 01, 01, 18, 00, 00) },
            };

            var matchResult = new MatchResult
            {
                EventStatus = MatchStatus.AbandonedStatus,
                HomeScore = 1,
                AwayScore = 2
            };

            var timelines = new List<ITimeline>
            {
                new Timeline { Type = "red_card", Time = new DateTime(2019, 01, 01, 18, 00, 00) },
            };

            var pushEvents = new Dictionary<string, MatchPushEvent>
            {
                {"1234", new MatchPushEvent { MatchResult = matchResult, TimeLines = timelines } }
            };

            // Act
            viewModel.OnReceivingMatchEvent("1", pushEvents);

            // Assert
            Assert.Equal(matchResult, viewModel.MatchViewModel.Match.MatchResult);
            Assert.Equal(timelines[0], viewModel.MatchViewModel.Match.LatestTimeline);

            var expectedTimelines = match.TimeLines.Concat(timelines).Distinct();
            Assert.Equal(expectedTimelines, viewModel.MatchViewModel.Match.TimeLines);
            Assert.Equal("Abandoned", viewModel.MatchViewModel.DisplayMatchStatus);
            Assert.Equal("1 - 2", viewModel.DisplayScore);
        }

        [Fact]
        public void OnReceivingMatchEvent_IsCurrentMatch_CurrentTimelinesIsNull_BuildGeneralInfo()
        {
            // Arrange
            var parameters = new NavigationParameters { { "Match", new Match { Id = "1234" } } };
            viewModel.OnNavigatingTo(parameters);

            var timelines = new List<ITimeline>
            {
                new Timeline { Type = "red_card", Time = new DateTime(2019, 01, 01, 18, 00, 00) },
            };

            var pushEvents = new Dictionary<string, MatchPushEvent>
            {
                {"1234", new MatchPushEvent {  TimeLines = timelines } }
            };

            // Act
            viewModel.OnReceivingMatchEvent("1", pushEvents);

            // Assert
            Assert.Equal(timelines, viewModel.MatchViewModel.Match.TimeLines);
        }

        [Fact]
        public void OnReceivingMatchEvent_IsNotCurrentMatch_Return()
        {
            // Arrange
            var timelines = new List<ITimeline>
            {
                new Timeline { Type = "red_card", Time = new DateTime(2019, 01, 01, 18, 00, 00) },
            };

            var pushEvents = new Dictionary<string, MatchPushEvent>
            {
                { "1", new MatchPushEvent{ TimeLines = timelines } }
            };

            // Act
            viewModel.OnReceivingMatchEvent("1", pushEvents);

            // Assert
            Assert.Null(viewModel.MatchViewModel.Match.TimeLines);
        }

        [Fact]
        public async Task RefreshCommand_OnExecuted_LoadGeneralInfo()
        {
            // Arrange
            matchService.GetMatch(viewModel.SettingsService.UserSettings, match.Id, true).Returns(match);

            // Act
            await viewModel.RefreshCommand.ExecuteAsync();

            // Assert
            Assert.Equal("01 Jan, 2019 - LALIGA", viewModel.DisplayEventDateAndLeagueName);
        }

        [Fact]
        public void OnDisappearing_Alway_Clean()
        {
            // Arrange
            viewModel.OnAppearing();

            // Act
            viewModel.OnDisappearing();
        }
    }
}