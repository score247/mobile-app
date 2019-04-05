namespace LiveScore.UITests.Pages
{
    using Xamarin.UITest;

    public abstract class BasePage
    {
        protected readonly IApp App;
        protected readonly bool OnAndroid;
        protected readonly bool OniOS;
        private readonly string pageTitle;

        protected BasePage(IApp app, Platform platform, string pageTitle)
        {
            App = app;

            OnAndroid = platform == Platform.Android;
            OniOS = platform == Platform.iOS;

            this.pageTitle = pageTitle;
        }

        public bool IsPageVisible => App.Query(pageTitle).Length > 0;

        public void WaitForPageToLoad()
        {
            App.WaitForElement(pageTitle);
        }
    }
}
