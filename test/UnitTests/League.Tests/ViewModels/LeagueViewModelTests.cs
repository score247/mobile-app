namespace League.Tests.ViewModels
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using League.Services;
    using League.ViewModels;
    using NSubstitute;
    using Prism.Navigation;
    using Prism.Services;
    using Xunit;
    using League.Models;
    using System.Linq;
    using System.Collections.ObjectModel;

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
            mockNavigationService.NavigateAsync(Arg.Any<string>()).Returns(new NavigationResult { Success = true });

            // Act
            await viewModel.ItemTappedCommand.ExecuteAsync(item);

            // Assert
            await mockNavigationService.Received(1).NavigateAsync(Arg.Any<string>(), Arg.Any<NavigationParameters>());
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

        [Fact]
        public async Task LoadLeaguesCommand_Executed_ShouldCallLeagueService()
        {
            // Arrange
            mockLeagueService.GetLeaguesAsync().Returns(new List<LeagueItem>());

            // Act
            await viewModel.LoadLeaguesCommand.ExecuteAsync();

            // Assert
            await mockLeagueService.Received(1).GetLeaguesAsync();
        }

        [Fact]
        public async Task LoadLeaguesCommand_Executed_ShouldRaisePropertyChanged()
        {
            // Arrange
            bool invoked = false;
            var mockLeagueItems = new List<LeagueItem>
            {
                new LeagueItem{ Id = "1", Name = "League A"}
            };

            mockLeagueService.GetLeaguesAsync().Returns(mockLeagueItems);

            viewModel.PropertyChanged += (sender, e) =>
            {
                if(e.PropertyName.Equals("Leagues"))
                {
                    invoked = true;
                }
            };

            // Act
            await viewModel.LoadLeaguesCommand.ExecuteAsync();

            // Assert
            Assert.True(invoked);
        }

        [Fact]
        public async Task IsLoading_ExecutedLoadLeaguesCommand_ShouldRaisePropertyChanged()
        {
            // Arrange
            bool invoked = false;
            var mockLeagueItems = new List<LeagueItem>
            {
                new LeagueItem{ Id = "1", Name = "League A"}
            };

            mockLeagueService.GetLeaguesAsync().Returns(mockLeagueItems);

            viewModel.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName.Equals("IsLoading"))
                {
                    invoked = true;
                }
            };

            // Act
            await viewModel.LoadLeaguesCommand.ExecuteAsync();

            // Assert
            Assert.True(invoked);
        }

        [Fact]
        public async Task SearchCommand_ExecutedWithEmptyQuery_ShouldReturnList()
        {
            // Arrange
            var mockLeagueItems = new List<LeagueItem>
            {
                new LeagueItem{ Id = "1", Name = "K League"},
                new LeagueItem{ Id = "1", Name = "Premier League"},
                new LeagueItem{ Id = "1", Name = "AFC"}
            };

            mockLeagueService.GetLeaguesAsync().Returns(mockLeagueItems);

            // Act
            await viewModel.LoadLeaguesCommand.ExecuteAsync();
            viewModel.SearchCommand.Execute();

            // Assert
            Assert.Equal(3, viewModel.Leagues.Count);
        }

        [Fact]
        public async Task SearchCommand_ExecutedWithQuery_ShouldReturnFilterdList()
        {
            // Arrange
            var mockLeagueItems = new List<LeagueItem>
            {
                new LeagueItem{ Id = "1", Name = "K League"},
                new LeagueItem{ Id = "1", Name = "Premier League"},
                new LeagueItem{ Id = "1", Name = "AFC"}
            };

            mockLeagueService.GetLeaguesAsync().Returns(mockLeagueItems);
            viewModel.Filter = "K";

            // Act
            await viewModel.LoadLeaguesCommand.ExecuteAsync();
            viewModel.SearchCommand.Execute();

            // Assert
            Assert.True(viewModel.Leagues.Count == 1 && viewModel.Leagues.FirstOrDefault().Name.Equals("K League"));
        }

        [Fact]
        public async Task RefreshCommand_Executed_ShouldCallLeagueService()
        {
            // Arrange
            mockLeagueService.GetLeaguesAsync().Returns(new List<LeagueItem>());

            // Act
            await viewModel.RefreshCommand.ExecuteAsync();

            // Assert
            await mockLeagueService.Received(1).GetLeaguesAsync();
        }

        [Fact]
        public async Task OnAppearing_LeaguesEmpty_ShouldCallLeagueService()
        {
            // Arrange
            mockLeagueService.GetLeaguesAsync().Returns(new List<LeagueItem>());

            // Act
            viewModel.OnAppearing();

            // Assert
            await mockLeagueService.Received(1).GetLeaguesAsync();
        }

        [Fact]
        public async Task OnAppearing_LeaguesNotEmpty_ShouldNotCallLeagueService()
        {
            // Arrange
            viewModel.Leagues = new ObservableCollection<LeagueItem>();
            viewModel.Leagues.Add(new LeagueItem { Id = "1", Name = "League A" });
            mockLeagueService.GetLeaguesAsync().Returns(new List<LeagueItem>());

            // Act
            viewModel.OnAppearing();

            // Assert
            await mockLeagueService.DidNotReceive().GetLeaguesAsync();
        }
    }
}
