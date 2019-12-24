using System.Collections.Generic;
using AutoFixture;
using LiveScore.Core.Converters;
using LiveScore.Core.Enumerations;
using LiveScore.Core.Models.Matches;
using LiveScore.Core.Services;
using LiveScore.Core.Tests.Fixtures;
using LiveScore.Soccer.Models.Matches;
using LiveScore.Soccer.ViewModels.Matches;
using NSubstitute;
using Prism.Navigation;
using Xunit;

namespace Soccer.Tests.ViewModels
{
    public class MatchDetailViewModelTests : IClassFixture<ViewModelBaseFixture>, IClassFixture<ResourcesFixture>
    {
        private readonly MatchDetailViewModel viewModel;
        private readonly IMatchService matchService;
        private readonly IFavoriteService<IMatch> favoriteService;

        //private readonly SoccerMatch match;
        private readonly Fixture fixture;

        public MatchDetailViewModelTests(ViewModelBaseFixture baseFixture)
        {
            fixture = baseFixture.CommonFixture.Specimens;

            matchService = Substitute.For<IMatchService>();
            baseFixture.DependencyResolver
                .Resolve<IMatchDisplayStatusBuilder>("1")
                .Returns(Substitute.For<IMatchDisplayStatusBuilder>());
            baseFixture.DependencyResolver.Resolve<IMatchService>("1").Returns(matchService);
            favoriteService = Substitute.For<IFavoriteService<IMatch>>();

            viewModel = new MatchDetailViewModel(
                baseFixture.NavigationService,
                baseFixture.DependencyResolver,
                baseFixture.EventAggregator);
        }

        [Fact]
        public void OnNavigatingTo_HasSecondLeg_ShowSecondLegMessage()
        {
            // Arrange
            var match = fixture.Create<SoccerMatch>()
                .With(match => match.AggregateHomeScore, (byte)2)
                .With(match => match.AggregateAwayScore, (byte)5)
                .With(match => match.EventStatus, MatchStatus.Closed);

            var parameters = new NavigationParameters { { "Match", match } };

            // Act
            viewModel.Initialize(parameters);

            // Assert
            Assert.Equal("2nd Leg, Aggregate Score: 2 - 5", viewModel.DisplaySecondLeg);
        }

        [Fact]
        public void OnNavigatingTo_HasSecondLeg_HomeTeamWin_ShowSecondLegImageInHome()
        {
            // Arrange
            var match = fixture.Create<SoccerMatch>()
                 .With(match => match.AggregateHomeScore, (byte)2)
                 .With(match => match.AggregateAwayScore, (byte)5)
                 .With(match => match.AggregateWinnerId, "sr:home")
                 .With(match => match.WinnerId, "sr:home")
                 .With(match => match.HomeTeamId, "sr:home")
                 .With(match => match.EventStatus, MatchStatus.Closed);

            var parameters = new NavigationParameters { { "Match", match } };

            // Act
            viewModel.Initialize(parameters);

            // Assert
            Assert.Equal("images/common/second_leg_winner.png", match.HomeSecondLegImage);
        }

        [Fact]
        public void OnNavigatingTo_HasSecondLeg_AwayTeamWin_ShowSecondLegImageInAway()
        {
            // Arrange
            var match = fixture.Create<SoccerMatch>()
                 .With(match => match.AggregateHomeScore, (byte)2)
                 .With(match => match.AggregateAwayScore, (byte)5)
                 .With(match => match.AggregateWinnerId, "sr:away")
                 .With(match => match.WinnerId, "sr:away")
                 .With(match => match.AwayTeamId, "sr:away")
                 .With(match => match.EventStatus, MatchStatus.Closed);

            var parameters = new NavigationParameters { { "Match", match } };

            // Act
            viewModel.Initialize(parameters);

            // Assert
            Assert.Equal("images/common/second_leg_winner.png", match.AwaySecondLegImage);
        }

        [Fact]
        public void OnNavigatingTo_HasPenalty_WinnerIsHome_ShowPenaltyImage()
        {
            // Arrange
            var match = fixture.Create<SoccerMatch>()
                 .With(match => match.WinnerId, "sr:home")
                 .With(match => match.HomeTeamId, "sr:home")
                 .With(match => match.EventStatus, MatchStatus.Closed)
                 .With(match => match.MatchPeriods, new List<MatchPeriod>
                    {
                        new MatchPeriod { HomeScore = 3, AwayScore = 4, PeriodType = PeriodType.Penalties }
                    });
            var parameters = new NavigationParameters { { "Match", match } };

            // Act
            viewModel.Initialize(parameters);

            // Assert
            Assert.Equal("images/common/penalty_winner.png", match.HomePenaltyImage);
        }

        [Fact]
        public void OnNavigatingTo_HasPenalty_WinnerIsAway_ShowPenaltyImage()
        {
            // Arrange
            var match = fixture.Create<SoccerMatch>()
                 .With(match => match.WinnerId, "sr:away")
                 .With(match => match.AwayTeamId, "sr:away")
                 .With(match => match.EventStatus, MatchStatus.Closed)
                 .With(match => match.MatchPeriods, new List<MatchPeriod>
                    {
                        new MatchPeriod { HomeScore = 3, AwayScore = 4, PeriodType = PeriodType.Penalties }
                    });
            var parameters = new NavigationParameters { { "Match", match } };

            // Act
            viewModel.Initialize(parameters);

            // Assert
            Assert.Equal("images/common/penalty_winner.png", match.AwayPenaltyImage);
        }
    }
}