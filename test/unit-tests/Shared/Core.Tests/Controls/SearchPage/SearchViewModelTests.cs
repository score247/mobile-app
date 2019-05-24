namespace LiveScore.Core.Tests.Controls.SearchPage
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using KellermanSoftware.CompareNetObjects;
    using LiveScore.Core.Controls.SearchPage;
    using LiveScore.Core.Tests.Fixtures;
    using Xunit;

    public class SearchViewModelTests : IClassFixture<ViewModelBaseFixture>
    {
        private readonly SearchViewModel searchViewModel;
        private readonly CompareLogic comparer;

        public SearchViewModelTests(ViewModelBaseFixture baseFixture)
        {
            comparer = baseFixture.CommonFixture.Comparer;
            searchViewModel = new SearchViewModel(
                baseFixture.NavigationService,
                baseFixture.DepdendencyResolver,
                baseFixture.EventAggregator);
        }

        [Fact]
        public void SearchText_Always_GetFromSetValue()
        {
            // Act
            searchViewModel.SearchText = "test";

            // Assert
            Assert.Equal("test", searchViewModel.SearchText);
        }

        [Fact]
        public void TextChangedCommand_OnTextChanged_GetExpectedSuggestionItem()
        {
            // Arrange
            searchViewModel.SearchText = "Ase";

            // Act
            searchViewModel.TextChangeCommand.Execute();

            // Assert
            var actualSuggestionItem = searchViewModel.SuggestionItemSource.ToList();
            var expectedSuggestionItem = new List<SearchSuggestion> { new SearchSuggestion { Name = "Asernal" } };
            Assert.True(comparer.Compare(expectedSuggestionItem, actualSuggestionItem).AreEqual);
        }

        [Fact]
        public void TextChangedCommand_OnTextChanged_SearchTextIsNullOrEmpty_ClearSuggestionList()
        {
            // Arrange
            searchViewModel.SearchText = "";

            // Act
            searchViewModel.TextChangeCommand.Execute();

            // Assert
            var actualSuggestionItem = searchViewModel.SuggestionItemSource;
            Assert.Null(actualSuggestionItem);
        }

        [Fact]
        public async Task CancelCommand_OnExecuted_CallNavigationServiceGoBack()
        {
            // Arrange
            var navigationService = searchViewModel.NavigationService as FakeNavigationService;

            // Act
            await searchViewModel.CancelCommand.ExecuteAsync();

            // Assert
            Assert.True(navigationService.UseModalNavigation);
            Assert.True(navigationService.IsGoBack);
        }
    }
}