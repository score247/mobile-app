
namespace LiveScoreApp.UITests.Tests
{
    using System;
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
        public void OpenLeaguesPage()
        {
            mainPage.OpenLeaguesTab();
            mainPage.WaitForLabel("Leagues");

            app.Screenshot("LeaguesPage");
        }

        [Test]
        public void OpenFavoritesPage()
        {
            mainPage.OpenFavoritesTab();
            mainPage.WaitForLabel("Favorites");

            app.Screenshot("FavoritesPage");
        }

        [Test]
        public void OpenLivePage()
        {
            mainPage.OpenLiveTab();
            mainPage.WaitForLabel("Live");
                       
            app.Screenshot("LivePage");
        }
    }
}
