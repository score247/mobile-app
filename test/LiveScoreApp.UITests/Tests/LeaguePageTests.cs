namespace LiveScore.UITests.Tests
{
    using NUnit.Framework;
    using Xamarin.UITest;

    public class LeaguePageTests : BaseTest
    {
        public LeaguePageTests(Platform platform) : base(platform)
        {
        }

        public override void BeforeEachTest()
        {
            base.BeforeEachTest();

            App.Tap(c => c.Class("UITabBarButton").Marked("Leagues"));

            LeaguePage.WaitForPageToLoad();
        }

        [Test]
        public void LeaguePageLaunches()
        {
            LeaguePage.WaitForNoActivityIndicator();
            App.Screenshot("LeaguePage");
        }

        [Ignore("render elements tree, only use for building tests")]
        [Test]
        public void Repl()
        {
            App.Repl();
        }
    }
}
