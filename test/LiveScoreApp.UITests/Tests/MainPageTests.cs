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

            mainPage.WaitForPageToLoad();
        }

        [Test]
        public void MainPageLaunches()
        {
            mainPage.WaitForLabel("Scores");

            app.Screenshot("ScorePage");
        }

        [Test]
        public void TappedLeaguesTab()
        {
            mainPage.OpenLeaguesTab();
            mainPage.WaitForLabel("Leagues");

            app.Screenshot("LeaguesPage");
        }

        [Test]
        public void TappedFavoritesTab()
        {
            mainPage.OpenFavoritesTab();
            mainPage.WaitForLabel("Favorites");

            app.Screenshot("FavoritesPage");
        }

        [Test]
        public void TappedLiveTap()
        {
            mainPage.OpenLiveTab();
            mainPage.WaitForLabel("Live");
                       
            app.Screenshot("LivePage");
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
            var currentTabs = mainPage.CurrentTabBars();

            // Assert
            Assert.IsTrue(expectedTabs.SequenceEqual(currentTabs));
        }
    }
}
