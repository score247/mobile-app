namespace Soccer.Tests.ViewModels
{
    using AutoFixture;
    using LiveScore.Core;
    using LiveScore.Core.Converters;
    using LiveScore.Core.Enumerations;
    using LiveScore.Core.Models.Leagues;
    using LiveScore.Core.Models.Matches;
    using LiveScore.Core.Services;
    using LiveScore.Core.Tests.Fixtures;
    using LiveScore.Soccer.Converters;
    using LiveScore.Soccer.ViewModels;
    using NSubstitute;
    using Prism.Events;
    using Prism.Navigation;
    using System;
    using System.Threading.Tasks;
    using Xunit;

    public class MatchDetailViewModelTests : IClassFixture<ViewModelBaseFixture>
    {
        private readonly Fixture specimens;
        private readonly MatchDetailViewModel viewModel;
        private readonly IMatch match;

        public MatchDetailViewModelTests(ViewModelBaseFixture baseFixture)
        {
            specimens = baseFixture.CommonFixture.Specimens;
            baseFixture.DependencyResolver.Resolve<IMatchStatusConverter>(
                baseFixture.AppSettingsFixture.SettingsService.CurrentSportType.Value)
                .Returns(new MatchStatusConverter());
            viewModel = new MatchDetailViewModel(
                baseFixture.NavigationService,
                baseFixture.DependencyResolver,
                baseFixture.EventAggregator,
                baseFixture.HubService);

            match = Substitute.For<IMatch>();
            match.MatchResult.EventStatus.Returns(new MatchStatus { Value = MatchStatus.Live });
            match.MatchResult.MatchStatus.Returns(new MatchStatus { Value = MatchStatus.FirstHaft });
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
        public void OnNavigatingTo_ParametersIsNotNull_BuildOtherGeneralInfo()
        {
            // Arrange
            match.EventDate.Returns(new DateTime(2019, 01, 01, 18, 00, 00));
            match.League.Name.Returns("Laliga");
            var parameters = new NavigationParameters { { "Match", match } };

            // Act
            viewModel.OnNavigatingTo(parameters);

            // Assert
            Assert.Equal("01 Jan, 2019 - LALIGA", viewModel.DisplayEventDateAndLeagueName);
        }
    }
}