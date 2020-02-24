using LiveScore.Core.Controls.CustomSearchBar;

namespace LiveScore.Core.Tests.Controls.SearchPage
{
    using KellermanSoftware.CompareNetObjects;
    using LiveScore.Core.Tests.Fixtures;
    using Xunit;

    public class SearchViewModelTests : IClassFixture<ViewModelBaseFixture>
    {
        private readonly SearchViewModel searchViewModel;
        private readonly CompareLogic comparer;

        public SearchViewModelTests(ViewModelBaseFixture baseFixture)
        {
            comparer = baseFixture.CommonFixture.Comparer;
            searchViewModel = new SearchViewModel();
        }

        [Fact]
        public void SearchText_Always_GetFromSetValue()
        {
            // Act
            searchViewModel.SearchText = "test";

            // Assert
            Assert.Equal("test", searchViewModel.SearchText);
        }
    }
}