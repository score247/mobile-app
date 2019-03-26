namespace LiveScoreApp.UITests.Tests
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

            scorePage.WaitForPageToLoad();
        }

        [Test]
        public void LeaguePageLaunches()
        {
            app.Screenshot("LeaguePage");
        }

        //[Ignore("render elements tree, only use for building tests")]
        [Test]
        public void Repl()
        {
            app.Repl();
        }
    }
}
