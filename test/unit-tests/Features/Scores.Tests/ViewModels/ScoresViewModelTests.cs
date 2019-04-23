namespace Scores.Tests.ViewModels
{
    using LiveScore.Core.Tests.Fixtures;
    using LiveScore.Score.ViewModels;
    using Xunit;

    public class ScoresViewModelTests : IClassFixture<ViewModelBaseFixture>
    {
        private readonly ScoresViewModel viewModel;

        public ScoresViewModelTests(ViewModelBaseFixture viewModelBaseFixture)
        {
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
    }
}