namespace LiveScoreApp.UITests.Tests
{
    using LiveScoreApp.UITests.Pages;
    using NUnit.Framework;
    using Xamarin.UITest;

    [TestFixture(Platform.iOS)]

    public abstract class BaseTest
    {
        protected IApp app;
        protected Platform platform;

        protected ScorePage scorePage;
        protected LeaguePage leaguePage;

        protected BaseTest(Platform platform)
        {
            this.platform = platform;
        }

        [SetUp]
        virtual public void BeforeEachTest()
        {
            app = AppInitializer.StartApp(platform);
            app.Screenshot("App Initialized");

            scorePage = new ScorePage(app, platform);

            //TODO tapped to League Icon
            leaguePage = new LeaguePage(app, platform);
        }
    }
}
