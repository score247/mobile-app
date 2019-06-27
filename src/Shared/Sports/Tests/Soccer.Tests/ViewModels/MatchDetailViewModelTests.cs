namespace Soccer.Tests.ViewModels
{
    using System;
    using System.Collections.Generic;
    using KellermanSoftware.CompareNetObjects;
    using LiveScore.Common.Services;
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
    using NSubstitute;
    using Prism.Navigation;
    using Xunit;

    public class MatchDetailViewModelTests : IClassFixture<ViewModelBaseFixture>, IClassFixture<ResourcesFixture>
    {
        private readonly MatchDetailViewModel viewModel;
        private readonly IMatchService matchService;
        private readonly Match match;
        private readonly ILocalStorage localStorage;

        public MatchDetailViewModelTests(ViewModelBaseFixture baseFixture)
        {
            localStorage = Substitute.For<ILocalStorage>();
            baseFixture.DependencyResolver.Resolve<IMatchStatusConverter>(
                baseFixture.AppSettingsFixture.SettingsService.CurrentSportType.Value)
                .Returns(new MatchStatusConverter(localStorage));
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
                AggregateWinnerId = "home",
                EventStatus = MatchStatus.ClosedStatus
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
                EventStatus = MatchStatus.ClosedStatus,
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
                EventStatus = MatchStatus.ClosedStatus,
                AggregateWinnerId = "away",
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
                },
                EventStatus = MatchStatus.ClosedStatus
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
                },
                EventStatus = MatchStatus.ClosedStatus
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
                },
                EventStatus = MatchStatus.ClosedStatus
            };
            var parameters = new NavigationParameters { { "Match", match } };

            // Act
            viewModel.OnNavigatingTo(parameters);

            // Assert
            Assert.Equal("images/common/penalty_winner.png", match.AwayPenaltyImage);
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
            Assert.Equal("Abandoned", viewModel.MatchViewModel.DisplayMatchStatus);
            Assert.Equal("1 - 2", viewModel.DisplayScore);
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
        public void OnDisappearing_Alway_Clean()
        {
            // Arrange
            viewModel.OnAppearing();

            // Act
            viewModel.OnDisappearing();
        }
    }
}