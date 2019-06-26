using System.Threading.Tasks;
using LiveScore.Core.Tests.Fixtures;
using LiveScore.ViewModels;
using Xunit;

namespace LiveScore.Tests.ViewModels
{
    public class TabMoreViewModelTests : IClassFixture<ViewModelBaseFixture>
    {
        private readonly TabMoreViewModel viewModel;

        public TabMoreViewModelTests(ViewModelBaseFixture baseFixture)
        {
            viewModel = new TabMoreViewModel(baseFixture.NavigationService, baseFixture.DependencyResolver);
        }

        [Fact]
        public async Task ItemTappedCommand_Execute_InjectNavigationService()
        {
            // Act
            await viewModel.ItemTappedCommand.ExecuteAsync(new Models.TabItem("", "view", ""));

            // Assert
            Assert.Equal("view", (viewModel.NavigationService as FakeNavigationService).NavigationPath);
        }

        [Fact]
        public async Task ItemTappedCommand_ExecuteNullItem_NotInjectNavigationService()
        {
            // Arrange

            // Act
            await viewModel.ItemTappedCommand.ExecuteAsync(null);

            // Assert
            Assert.Null((viewModel.NavigationService as FakeNavigationService).NavigationPath);
        }
    }
}