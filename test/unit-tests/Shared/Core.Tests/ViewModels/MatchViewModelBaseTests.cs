namespace LiveScore.Core.Tests.ViewModels
{
    using LiveScore.Core.Tests.Fixtures;
    using LiveScore.Core.ViewModels;
    using Xunit;

    public class MatchViewModelBaseTests : IClassFixture<ViewModelBaseFixture>
    {
        private readonly MatchViewModelBase viewModel;

        public MatchViewModelBaseTests(ViewModelBaseFixture baseFixture)
        {
            viewModel = new MatchViewModelBase(baseFixture.NavigationService, baseFixture.DepdendencyResolver);
        }

        [Fact]
        public void MatchId_Always_GetSetCorrectValue()
        {
            // Arrange
            viewModel.MatchId = "1";

            // Act
            var actual = viewModel.MatchId;

            // Assert
            Assert.Equal("1", actual);
        }
    }
}