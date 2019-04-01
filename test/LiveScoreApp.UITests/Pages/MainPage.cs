namespace LiveScoreApp.UITests.Pages
{
    using System.Linq;
    using Xamarin.UITest;
    using Query = System.Func<Xamarin.UITest.Queries.AppQuery, Xamarin.UITest.Queries.AppQuery>;

    public class MainPage : BasePage
    {
        private readonly Query tabBars;
        private Query navigationTitle;

        public MainPage(IApp app, Platform platform) : base(app, platform, "Scores")
        {
            tabBars = x => x.Class("UITabBarButton");
        }

        public void OpenLeaguesTab()
        {
            App.Tap(c => c.Class("UITabBarButton").Marked("Leagues"));
        }

        public void OpenLiveTab()
        {
            App.Tap(c => c.Class("UITabBarButton").Marked("Live"));
        }

        public void OpenFavoritesTab()
        {
            App.Tap(c => c.Class("UITabBarButton").Marked("Favorites"));
        }

        public void WaitForLabel(string title)
        {
            navigationTitle = x => x.Class("UILabel").Marked("NavigationTitle").Text(title);
            App.WaitForElement(navigationTitle);
        }

        public string[] CurrentTabBars()
        {
            var tabs = App.Query(x => x.Class("UITabBarButton"));

            return tabs.Select(x => x.Label).ToArray();
        }
    }
}
