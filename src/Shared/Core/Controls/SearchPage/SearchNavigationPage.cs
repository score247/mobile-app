namespace LiveScore.Core.Controls.SearchPage
{
    using Xamarin.Forms;

    public class SearchNavigationPage : NavigationPage
    {
        public SearchNavigationPage(Page page) : base(page)
        {
            BarTextColor = Color.White;
            BarBackgroundColor = (Color)Application.Current.Resources["SecondColor"];
        }
    }
}