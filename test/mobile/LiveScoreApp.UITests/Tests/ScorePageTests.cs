namespace LiveScore.UITests.Tests
{
    using NUnit.Framework;
    using Xamarin.UITest;

    public class ScorePageTests : BaseTest
    {
        public ScorePageTests(Platform platform) : base(platform)
        {
        }

        public override void BeforeEachTest()
        {
            base.BeforeEachTest();

            ScorePage.WaitForPageToLoad();
        }

        [Test]
        public void ScorePageLaunches()
        {
            App.Screenshot("ScorePage");
        }

        [Ignore("render elements tree, only use for building tests")]
        [Test]
        public void Repl()
        {
            App.Repl();
        }
    }
}
