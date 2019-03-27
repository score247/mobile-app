namespace LiveScoreApp.UITests.Pages
{
    using System;
    using Xamarin.UITest;
    using Xamarin.UITest.Queries;

    using Query = System.Func<Xamarin.UITest.Queries.AppQuery, Xamarin.UITest.Queries.AppQuery>;

    public class MainPage : BasePage
    {
        //public readonly AppResult[] tabBars;
        readonly Query tabBars;
        Query navigationTitle;

        public MainPage(IApp app, Platform platform) : base(app, platform, "Scores")
        {
            tabBars = x => x.Class("UITabBarButton");

        }

        public void OpenLeaguesTab()
        {
            app.Tap(c => c.Class("UITabBarButton").Marked("Leagues"));
        }

        public void OpenLiveTab()
        {
            app.Tap(c => c.Class("UITabBarButton").Marked("Live"));
        }

        public void OpenFavoritesTab()
        {
            app.Tap(c => c.Class("UITabBarButton").Marked("Favorites"));
        }

        public void WaitForLabel(string title) 
        {
            navigationTitle = x => x.Class("UILabel").Marked("NavigationTitle").Text(title);
            app.WaitForElement(navigationTitle);
        }
    }
}
