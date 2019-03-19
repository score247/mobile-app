namespace LiveScoreApp.UITests.Pages
{   
    using Xamarin.UITest;
    using Xamarin.UITest.Queries;

    public class ScorePage : BasePage
    {
        public readonly AppResult[] tabBars;
        public readonly AppResult[] sportSelect;

        public ScorePage(IApp app, Platform platform) : base(app, platform, "Scores")
        {
            tabBars = app.Query(c => c.Class("UITabBarButton"));
        }
    }
}
