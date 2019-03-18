namespace LiveScoreApp.UITests.Pages
{   
    using Xamarin.UITest;

    using Query = System.Func<Xamarin.UITest.Queries.AppQuery, Xamarin.UITest.Queries.AppQuery>;

    public class ScorePage : BasePage
    {
        readonly Query tabbedBar;


        public ScorePage(IApp app, Platform platform) : base(app, platform, "Scores")
        {
            //tabbedBar = x => x.Marked("Add");
        }
    }
}
