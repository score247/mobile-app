using LiveScore.Core.Tests.Fixtures;
using LiveScore.Core.ViewModels;
using Xunit;

namespace LiveScore.Core.Tests.ViewModels
{
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
        public void Init_ShouldReturnDefaultSportType()
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
