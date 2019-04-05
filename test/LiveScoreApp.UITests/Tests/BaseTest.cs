namespace LiveScore.UITests.Tests
{
    using LiveScore.UITests.Pages;
    using NUnit.Framework;
    using Xamarin.UITest;

    [TestFixture(Platform.iOS)]
    public abstract class BaseTest
    {
        private IApp app;
        private Platform platform;
        private MainPage mainPage;
        private ScorePage scorePage;
        private LeaguePage leaguePage;

        protected BaseTest(Platform platform)
        {
            Platform = platform;
        }

        protected IApp App { get => app; set => app = value; }

        protected Platform Platform { get => platform; set => platform = value; }

        protected MainPage MainPage { get => mainPage; set => mainPage = value; }

        protected ScorePage ScorePage { get => scorePage; set => scorePage = value; }

        protected LeaguePage LeaguePage { get => leaguePage; set => leaguePage = value; }

        [SetUp]
        public virtual void BeforeEachTest()
        {
            App = AppInitializer.StartApp(Platform);

            MainPage = new MainPage(App, Platform);
            ScorePage = new ScorePage(App, Platform);
            LeaguePage = new LeaguePage(App, Platform);
        }
    }
}
