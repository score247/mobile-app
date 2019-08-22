namespace Soccer.Tests.ViewModels
{
    using System;
    using System.Collections.Generic;
    using AutoFixture;
    using LiveScore.Common.Services;
    using LiveScore.Common.Tests.Extensions;
    using LiveScore.Core.Converters;
    using LiveScore.Core.Enumerations;
    using LiveScore.Core.Models.Matches;
    using LiveScore.Core.Services;
    using LiveScore.Core.Tests.Fixtures;
    using LiveScore.Soccer.Converters;
    using LiveScore.Soccer.Models.Leagues;
    using LiveScore.Soccer.Models.Matches;
    using LiveScore.Soccer.Models.Teams;
    using LiveScore.Soccer.ViewModels;
    using NSubstitute;
    using Prism.Navigation;
    using Xunit;

    public class MatchDetailViewModelTests : IClassFixture<ViewModelBaseFixture>, IClassFixture<ResourcesFixture>
    {
        private readonly MatchDetailViewModel viewModel;
        private readonly IMatchService matchService;
        private readonly MatchOld match;
        private readonly ICachingService localStorage;
        private Fixture specimens;

        public MatchDetailViewModelTests(ViewModelBaseFixture baseFixture)
        {
            specimens = new Fixture();
            localStorage = Substitute.For<ICachingService>();
            baseFixture.DependencyResolver.Resolve<IMatchStatusConverter>("1")
                .Returns(new MatchStatusConverter(localStorage));
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

        private MatchOld CreateMatch()
        {
            var matchData = new MatchOld
            {
                Id = "1234",
                League = new League { Name = "Laliga" },
                EventDate = new DateTime(2019, 01, 01, 18, 00, 00),
                Attendance = 2034,
                Venue = new Venue { Name = "My Dinh" },
                MatchResult = specimens
                    .For<MatchResult>()
                    .With(x => x.EventStatus, MatchStatus.Live)
                    .With(x => x.MatchStatus, MatchStatus.FirstHalf)
                    .With(x => x.HomeScore, (byte)5)
                    .With(x => x.AwayScore, (byte)1)
                    .Create(),
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
            match.LatestTimeline = new TimelineEvent { StoppageTime = "4", InjuryTimeAnnounced = 5 };
            match.MatchResult.UpdateMatchTime(49);
            var parameters = new NavigationParameters { { "Match", match } };

            // Act
            viewModel.OnNavigatingTo(parameters);

            // Assert
            Assert.Equal("45+4'", viewModel.MatchViewModel.DisplayMatchStatus);
        }

        [Fact]
        public void OnNavigatingTo_HasSecondLeg_ShowSecondLegMessage()
        {
            // Arrange
            specimens = new Fixture();
            match.MatchResult = specimens
                   .For<MatchResult>()
                   .With(x => x.EventStatus, MatchStatus.Closed)
                   .With(x => x.AggregateHomeScore, (byte)2)
                   .With(x => x.AggregateAwayScore, (byte)5)
                   .With(x => x.AggregateWinnerId, "home")
                   .Create();

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
            specimens = new Fixture();
            match.MatchResult = specimens
                 .For<MatchResult>()
                 .With(x => x.EventStatus, MatchStatus.Closed)
                 .With(x => x.AggregateHomeScore, (byte)2)
                 .With(x => x.WinnerId, "home")
                 .With(x => x.AggregateWinnerId, "home")
                 .Create();
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
            specimens = new Fixture();
            match.MatchResult = specimens
               .For<MatchResult>()
               .With(x => x.EventStatus, MatchStatus.Closed)
               .With(x => x.WinnerId, "away")
               .With(x => x.AggregateWinnerId, "away")
               .Create();

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
            specimens = new Fixture();
            match.MatchResult = specimens
                 .For<MatchResult>()
                 .With(x => x.EventStatus, MatchStatus.Closed)
                 .With(x => x.MatchPeriods, new List<MatchPeriod>
                    {
                        new MatchPeriod { HomeScore = 3, AwayScore = 4, PeriodType = PeriodType.Penalties }
                    })
                 .With(x => x.WinnerId, "home")
                 .Create();

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
            specimens = new Fixture();
            match.MatchResult = specimens
                 .For<MatchResult>()
                 .With(x => x.EventStatus, MatchStatus.Closed)
                 .With(x => x.MatchPeriods, new List<MatchPeriod>
                    {
                        new MatchPeriod { HomeScore = 3, AwayScore = 4, PeriodType = PeriodType.Penalties }
                    })
                 .With(x => x.WinnerId, "away")
                 .Create();

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
            specimens = new Fixture();

            var matchResult = specimens
                .For<MatchResult>()
                .With(x => x.EventStatus, MatchStatus.Abandoned)
                .With(x => x.HomeScore, (byte)1)
                .With(x => x.AwayScore, (byte)2)
                .Create();

            var timeline = new TimelineEvent { Type = EventType.RedCard, Time = new DateTime(2019, 01, 01, 18, 00, 00) };
            var matchEvent = new MatchEvent("1234", matchResult, timeline);

            // Act
            viewModel.OnReceivedMatchEvent(1, matchEvent);

            //// Assert
            // TODO: update later
            // Assert.Equal(matchResult, viewModel.MatchViewModel.Match.MatchResult);
            // Assert.Equal(timeline, viewModel.MatchViewModel.Match.LatestTimeline);
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
            // TODO: update later
            //Assert.Null(viewModel.MatchViewModel.Match.TimeLines);
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