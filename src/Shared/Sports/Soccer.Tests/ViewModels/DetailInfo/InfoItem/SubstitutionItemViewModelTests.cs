using AutoFixture;
using LiveScore.Core;
using LiveScore.Core.Tests.Fixtures;
using LiveScore.Soccer.Models.Matches;
using LiveScore.Soccer.ViewModels.MatchDetails.Information.InfoItems;
using Prism.Navigation;
using Xunit;

namespace Soccer.Tests.ViewModels.DetailInfo.InfoItem
{
    public class SubstitutionItemViewModelTests : IClassFixture<ViewModelBaseFixture>
    {
        private readonly Fixture fixture;
        private readonly INavigationService navigationService;
        private readonly IDependencyResolver dependencyResolver;

        public SubstitutionItemViewModelTests(ViewModelBaseFixture baseFixture)
        {
            fixture = baseFixture.CommonFixture.Specimens;
            navigationService = baseFixture.NavigationService;
            dependencyResolver = baseFixture.DependencyResolver;
        }

        [Fact]
        public void BuildData_Home_AssignedHomePlayers() 
        {
            // Arrange
            var timeline = fixture.Create<TimelineEvent>().With(timeline => timeline.Team, "home");
            var viewModel = new SubstitutionItemViewModel(
                timeline,
                fixture.Create<MatchInfo>(),
                navigationService,
                dependencyResolver);

            // Act
            viewModel.BuildData();

            // Assert
            Assert.Equal(viewModel.HomePlayerOutName, timeline.PlayerOut.Name);
            Assert.Equal(viewModel.HomePlayerInName, timeline.PlayerIn.Name);
            Assert.True(viewModel.VisibleHomeImage);
        }

        [Fact]
        public void BuildData_Away_AssignedAwayPlayers()
        {
            // Arrange
            var timeline = fixture.Create<TimelineEvent>().With(timeline => timeline.Team, "away");
            var viewModel = new SubstitutionItemViewModel(
                timeline,
                fixture.Create<MatchInfo>(),
                navigationService,
                dependencyResolver);

            // Act
            viewModel.BuildData();

            // Assert
            Assert.Equal(viewModel.AwayPlayerOutName, timeline.PlayerOut.Name);
            Assert.Equal(viewModel.AwayPlayerInName, timeline.PlayerIn.Name);
            Assert.True(viewModel.VisibleAwayImage);
        }
    }
}
