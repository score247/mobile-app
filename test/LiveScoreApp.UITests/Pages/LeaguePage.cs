namespace LiveScoreApp.UITests.Pages
{
    using Xamarin.UITest;
    using Xamarin.UITest.Queries;

    public class LeaguePage : BasePage
    {
        public readonly AppResult[] tabBars;
        public readonly AppResult[] sportSelect;

        public LeaguePage(IApp app, Platform platform) : base(app, platform, "Leagues")
        {
            tabBars = app.Query(c => c.Class("UITabBarButton"));

            //app.Tap(addToolbarButton);
        }
    }
}
