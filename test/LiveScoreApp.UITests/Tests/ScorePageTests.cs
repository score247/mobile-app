namespace LiveScoreApp.UITests.Tests
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

            scorePage.WaitForPageToLoad();
        }

        [Test]
        public void ScorePageLaunches()
        {
            app.Screenshot("ScorePage");
        }
    }
}
