using LiveScore.Core;
using LiveScore.ViewModels;
using NSubstitute;
using Prism.Navigation;
using System.Threading.Tasks;
using Xunit;

namespace LiveScore.Tests.ViewModels
{
    public class TabMoreViewModelTests
    {
        private INavigationService mockNavigation;
        private IDepdendencyResolver mockResolver;

        private readonly TabMoreViewModel viewModel;

        public TabMoreViewModelTests()
        {
            mockNavigation = Substitute.For<INavigationService>();

            viewModel = new TabMoreViewModel(mockNavigation, mockResolver);
        }

        [Fact]
        public async Task ItemTappedCommand_Execute_InjectNavigationService()
        {
            // Arrange

            // Act
            await viewModel.ItemTappedCommand.ExecuteAsync(new Models.TabItem("", "", ""));

            // Assert
            await mockNavigation.Received(1).NavigateAsync(Arg.Any<string>());
        }

        [Fact]
        public async Task ItemTappedCommand_ExecuteNullItem_NotInjectNavigationService()
        {
            // Arrange

            // Act
            await viewModel.ItemTappedCommand.ExecuteAsync(null);

            // Assert
            await mockNavigation.DidNotReceive().NavigateAsync(Arg.Any<string>());
        }
    }
}
