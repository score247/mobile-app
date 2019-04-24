namespace Scores.Tests.ViewModels
{
    using AutoFixture;
    using KellermanSoftware.CompareNetObjects;
    using LiveScore.Common.Extensions;
    using LiveScore.Core.Models.Matches;
    using LiveScore.Core.Services;
    using LiveScore.Core.Tests.Fixtures;
    using LiveScore.Score.ViewModels;
    using NSubstitute;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Xunit;

    public class ScoresViewModelTests : IClassFixture<ViewModelBaseFixture>
    {
        private readonly ScoresViewModel viewModel;
        private readonly IMatchService matchService;
        private readonly IList<IMatch> matchData;
        private readonly CompareLogic comparer;

        public ScoresViewModelTests(ViewModelBaseFixture viewModelBaseFixture)
        {
            comparer = viewModelBaseFixture.CommonFixture.Comparer;

            matchData = viewModelBaseFixture.CommonFixture.Fixture
                .CreateMany<IMatch>().ToList();

            matchService = Substitute.For<IMatchService>();

            viewModelBaseFixture.DepdendencyResolver
               .Resolve<IMatchService>(Arg.Any<string>())
               .Returns(matchService);

            viewModel = new ScoresViewModel(
                viewModelBaseFixture.NavigationService,
                viewModelBaseFixture.DepdendencyResolver,
                viewModelBaseFixture.EventAggregator);
        }

        [Fact]
        public void IsLoading_Always_GetExpectedSetValue()
        {
            // Arrange
            viewModel.IsLoading = true;

            // Act
            var actual = viewModel.IsLoading;

            // Assert
            Assert.True(actual);
        }

        [Fact]
        public void IsNotLoading_Always_GetExpectedSetValue()
        {
            // Arrange
            viewModel.IsLoading = true;

            // Act
            var actual = viewModel.IsNotLoading;

            // Assert
            Assert.False(actual);
        }

        [Fact]
        public void IsRefreshing_Always_GetExpectedSetValue()
        {
            // Arrange
            viewModel.IsRefreshing = true;

            // Act
            var actual = viewModel.IsRefreshing;

            // Assert
            Assert.True(actual);
        }

        [Fact]
        public async Task RefreshCommand_OnExecuting_RefreshMatchListItemSourceData()
        {
            // Arrange
            matchService.GetMatches(viewModel.SettingsService.UserSettings, Arg.Any<DateRange>(), true).Returns(matchData);

            // Act
            await viewModel.RefreshCommand.ExecuteAsync();

            // Assert
            var actualMatchData = viewModel.MatchItemSource.SelectMany(group => group).ToList();
            Assert.True(comparer.Compare(matchData, actualMatchData).AreEqual);
        }

        [Fact]
        public async Task OnNavigatingTo_Always_SubsribeDateBarItemSelectedEvent()
        {
        }
    }
}