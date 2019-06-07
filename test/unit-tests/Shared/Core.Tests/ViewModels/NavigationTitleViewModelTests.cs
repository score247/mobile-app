namespace LiveScore.Core.Tests.ViewModels
{
    using System.Threading.Tasks;
    using LiveScore.Core.Tests.Fixtures;
    using LiveScore.Core.ViewModels;
    using Xunit;

    public class NavigationTitleViewModelTests : IClassFixture<ViewModelBaseFixture>
    {
        private readonly NavigationTitleViewModel viewModel;

        public NavigationTitleViewModelTests(ViewModelBaseFixture viewModelBaseFixture)
        {
            viewModel = new NavigationTitleViewModel(
                viewModelBaseFixture.NavigationService,
                viewModelBaseFixture.DepdendencyResolver);
        }

        [Fact]
        public void CurrentSportName_Always_ReturnDefaultSportType()
        {
            // Arrange
            var expected = viewModel.SettingsService.CurrentSportType.DisplayName;

            // Act
            var sportName = viewModel.CurrentSportName;

            // Assert
            Assert.Equal(expected, sportName);
        }
    }
}