namespace Scores.Tests.ViewModels
{
    using LiveScore.Core;
    using LiveScore.Score.ViewModels;
    using NSubstitute;
    using Prism.Events;
    using Prism.Navigation;
    using Xunit;

    public class ScoresViewModelTests
    {
        private readonly INavigationService subNavigationService;
        private readonly IDepdendencyResolver subDependencyResolver;
        private readonly IEventAggregator subEventAggregator;

        public ScoresViewModelTests()
        {
            subNavigationService = Substitute.For<INavigationService>();
            subDependencyResolver = Substitute.For<IDepdendencyResolver>();
            subEventAggregator = Substitute.For<IEventAggregator>();
        }

        private ScoresViewModel CreateViewModel()
            => new ScoresViewModel(
                subNavigationService,
                subDependencyResolver,
                subEventAggregator);

        [Fact]
        public void IsLoading_Always_GetExpectedSetValue()
        {
            // Arrange
            var viewModel = CreateViewModel();
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
            var viewModel = CreateViewModel();
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
            var viewModel = CreateViewModel();
            viewModel.IsRefreshing = true;

            // Act
            var actual = viewModel.IsRefreshing;

            // Assert
            Assert.True(actual);
        }
    }
}