namespace LiveScoreApp.UITests.Pages
{
    using Xamarin.UITest;
    using Xamarin.UITest.Queries;

    using Query = System.Func<Xamarin.UITest.Queries.AppQuery, Xamarin.UITest.Queries.AppQuery>;

    public class LeaguePage : BasePage
    {
        public readonly AppResult[] tabBars;
        public readonly AppResult[] sportSelect;

        readonly Query leagueList, activityIndicator;

        public LeaguePage(IApp app, Platform platform) : base(app, platform, "Leagues")
        {
            activityIndicator = x => x.Marked("LeagueIndicator");
        }

        public void WaitForActivityIndicator()
        {
            app.WaitForElement(activityIndicator);
            app.Screenshot("Activity Indicator Appeared");
        }

        public void WaitForNoActivityIndicator()
        {
            app.WaitForNoElement(activityIndicator);
            app.Screenshot("Activity Indicator Disappeared");
        }
    }
}
