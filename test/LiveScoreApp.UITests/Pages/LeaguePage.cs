namespace LiveScoreApp.UITests.Pages
{
    using Xamarin.UITest;
    using Xamarin.UITest.Queries;

    using Query = System.Func<Xamarin.UITest.Queries.AppQuery, Xamarin.UITest.Queries.AppQuery>;

    public class LeaguePage : BasePage
    {
        private readonly Query activityIndicator;       

        public LeaguePage(IApp app, Platform platform) : base(app, platform, "Leagues")
        {
            activityIndicator = x => x.Marked("LeagueIndicator");
        }

        public void WaitForActivityIndicator()
        {
            App.WaitForElement(activityIndicator);
            App.Screenshot("Activity Indicator Appeared");
        }

        public void WaitForNoActivityIndicator()
        {
            App.WaitForNoElement(activityIndicator);
            App.Screenshot("Activity Indicator Disappeared");
        }
    }
}
