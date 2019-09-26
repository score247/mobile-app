using System;
using System.Collections.Generic;
using LiveScore.Core.Converters;
using LiveScore.Core.Enumerations;
using LiveScore.Core.Models.Matches;
using LiveScore.Core.Services;
using LiveScore.Core.Tests.Fixtures;
using LiveScore.Soccer.Converters;
using LiveScore.Soccer.Models.Leagues;
using LiveScore.Soccer.Models.Matches;
using LiveScore.Soccer.Models.Teams;
using LiveScore.Soccer.ViewModels.MatchDetails;
using NSubstitute;
using Prism.Navigation;
using Xunit;

namespace Soccer.Tests.ViewModels
{
    public class MatchDetailViewModelTests : IClassFixture<ViewModelBaseFixture>, IClassFixture<ResourcesFixture>
    {
        private readonly MatchDetailViewModel viewModel;
        private readonly IMatchService matchService;
        private readonly Match match;

        public MatchDetailViewModelTests(ViewModelBaseFixture baseFixture)
        {
            baseFixture.DependencyResolver
                .Resolve<IMatchStatusConverter>("1")
                .Returns(new MatchStatusConverter());
            matchService = Substitute.For<IMatchService>();
            baseFixture.DependencyResolver.Resolve<IMatchService>("1").Returns(matchService);

            var hubConnection = Substitute.For<FakeHubConnection>();
            var hubService = Substitute.For<IHubService>();
            hubService.BuildMatchEventHubConnection().Returns(hubConnection);
            baseFixture.DependencyResolver
                .Resolve<IHubService>("1")
                .Returns(hubService);

            viewModel = new MatchDetailViewModel(
                baseFixture.NavigationService,
                baseFixture.DependencyResolver,
                baseFixture.EventAggregator);

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
                    EventStatus = MatchStatus.Live,
                    MatchStatus = MatchStatus.FirstHalf
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
        public void OnNavigatingTo_HasSecondLeg_ShowSecondLegMessage()
        {
            // Arrange
            match.MatchResult = new MatchResult
            {
                AggregateHomeScore = 2,
                AggregateAwayScore = 5,
                AggregateWinnerId = "home",
                EventStatus = MatchStatus.Closed
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
                EventStatus = MatchStatus.Closed,
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
                EventStatus = MatchStatus.Closed,
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
        public void OnNavigatingTo_HasPenalty_WinnerIsHome_ShowPenaltyImage()
        {
            // Arrange
            match.MatchResult = new MatchResult
            {
                WinnerId = "home",
                MatchPeriods = new List<MatchPeriod>
                {
                    new MatchPeriod { HomeScore = 3, AwayScore = 4, PeriodType = PeriodType.Penalties }
                },
                EventStatus = MatchStatus.Closed
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
                    new MatchPeriod { HomeScore = 3, AwayScore = 4, PeriodType = PeriodType.Penalties }
                },
                EventStatus = MatchStatus.Closed
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
            match.TimeLines = new List<ITimelineEvent>
            {
                new TimelineEvent { Type = EventType.YellowCard, Time = new DateTime(2019, 01, 01, 18, 00, 00) },
            };

            var matchResult = new MatchResult
            {
                EventStatus = MatchStatus.Abandoned,
                HomeScore = 1,
                AwayScore = 2
            };

            var timeline = new TimelineEvent { Type = EventType.RedCard, Time = new DateTime(2019, 01, 01, 18, 00, 00) };
            var matchEvent = new MatchEvent("1234", matchResult, timeline);

            // Act
            viewModel.OnReceivedMatchEvent(1, matchEvent);

            // Assert
            Assert.Equal(matchResult, viewModel.MatchViewModel.Match.MatchResult);
            Assert.Equal(timeline, viewModel.MatchViewModel.Match.LatestTimeline);
            Assert.Equal("AB", viewModel.MatchViewModel.DisplayMatchStatus);
        }

        [Fact]
        public void OnReceivingMatchEvent_IsNotCurrentMatch_Return()
        {
            // Arrange
            var timeline = new TimelineEvent { Type = EventType.RedCard, Time = new DateTime(2019, 01, 01, 18, 00, 00) };
            var matchEvent = new MatchEvent("1", null, timeline);

            // Act
            viewModel.OnReceivedMatchEvent(1, matchEvent);

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