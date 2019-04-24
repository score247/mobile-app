namespace Scores.Tests.ViewModels
{
    using LiveScore.Core.Tests.Fixtures;
    using LiveScore.Score.ViewModels;
    using Xunit;

    public class MatchTrackerViewModelTests : IClassFixture<ViewModelBaseFixture>
    {
        private readonly MatchTrackerViewModel viewModel;

        public MatchTrackerViewModelTests(ViewModelBaseFixture baseFixture)
        {
            viewModel = new MatchTrackerViewModel(baseFixture.NavigationService, baseFixture.DepdendencyResolver);
        }

        [Fact]
        public void TestMethod1()
        {
            // Arrange

            // Act

            // Assert
        }
    }
}