using League.Models;
namespace League.Tests.ViewModels
{
    using System.Threading.Tasks;
    using League.Services;
    using League.ViewModels;
    using NSubstitute;
    using Prism.Navigation;
    using Prism.Services;
    using Xunit;

    public class LeagueViewModelTests
    {
        private readonly INavigationService mockNavigationService;
        private readonly ILeagueService mockLeagueService;
        private readonly IPageDialogService mockPageDialog;

        private LeagueViewModel viewModel;

        public LeagueViewModelTests()
        {
            mockLeagueService = Substitute.For<ILeagueService>();
            mockNavigationService = Substitute.For<INavigationService>();
            mockPageDialog = Substitute.For<IPageDialogService>();

            viewModel = new LeagueViewModel(mockNavigationService, mockLeagueService, mockPageDialog);
        }

        [Fact]
        public void Title_Instantiated_ShouldCorrect()
        {
            // Arrange
            var title = "League";

            // Act
            var actualTitle = viewModel.Title;

            // Assert
            Assert.Equal(title, actualTitle);
        }

        [Fact]
        public async Task ItemTappedCommand_Executed_ShouldCallNavigationService()
        {
            // Arrange
            var item = new LeagueItem();
            mockNavigationService.NavigateAsync(Arg.Any<string>()).Returns(new NavigationResult { Success = true});

            // Act
            await viewModel.ItemTappedCommand.ExecuteAsync(item);

            // Assert
            await mockNavigationService.Received(1).NavigateAsync(Arg.Any<string>());
        }

        [Fact]
        public async Task ItemTappedCommand_ExecutedFailed_ShouldDisplayAlert()
        {
            // Arrange
            var item = new LeagueItem();
            mockNavigationService.NavigateAsync(Arg.Any<string>()).Returns(new NavigationResult());

            // Act
            await viewModel.ItemTappedCommand.ExecuteAsync(item);

            // Assert
            await mockPageDialog.Received(1).DisplayAlertAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>());
        }
    }
}
