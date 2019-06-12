namespace Scores.Tests.ViewModels
{
    using LiveScore.Core.Tests.Fixtures;
    using LiveScore.Score.ViewModels;
    using Xunit;

    public class LiveViewModelTests : IClassFixture<ViewModelBaseFixture>
    {
        private readonly LiveViewModel viewModel;

        public LiveViewModelTests(ViewModelBaseFixture baseFixture)
        {
            viewModel = new LiveViewModel(baseFixture.NavigationService, baseFixture.DependencyResolver);
        }

        [Fact]
        public void Test()
        {
            // Assert
        }
    }
}