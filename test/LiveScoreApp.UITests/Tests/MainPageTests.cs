namespace LiveScoreApp.UITests.Tests
{
    using System.Linq;
    using NUnit.Framework;
    using Xamarin.UITest;

    public class MainPageTests : BaseTest
    {
        public MainPageTests(Platform platform) : base(platform)
        {
        }

        public override void BeforeEachTest()
        {
            base.BeforeEachTest();

            MainPage.WaitForPageToLoad();
        }

        [Test]
        public void MainPageLaunches()
        {
            MainPage.WaitForLabel("Scores");

            App.Screenshot("ScorePage");
        }

        [Test]
        public void TappedLeaguesTab()
        {
            MainPage.OpenLeaguesTab();
            MainPage.WaitForLabel("Leagues");

            App.Screenshot("LeaguesPage");
        }

        [Test]
        public void TappedFavoritesTab()
        {
            MainPage.OpenFavoritesTab();
            MainPage.WaitForLabel("Favorites");

            App.Screenshot("FavoritesPage");
        }

        [Test]
        public void TappedLiveTap()
        {
            MainPage.OpenLiveTab();
            MainPage.WaitForLabel("Live");

            App.Screenshot("LivePage");
        }

        [Test]
        public void MainPage_Launched_DisplayTabBars()
        {
            // Arrange
            var expectedTabs = new[]
            {
                "Scores",
                "Live",
                "Favorites",
                "Leagues",
                "More"
            };

            // Act
            var currentTabs = MainPage.CurrentTabBars();

            // Assert
            Assert.IsTrue(expectedTabs.SequenceEqual(currentTabs));
        }
    }
}
