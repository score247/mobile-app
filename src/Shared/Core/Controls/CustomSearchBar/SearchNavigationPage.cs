using Xamarin.Forms;

namespace LiveScore.Core.Controls.CustomSearchBar
{
    public class SearchNavigationPage : NavigationPage
    {
        public SearchNavigationPage(Page page) : base(page)
        {
            BarTextColor = Color.White;
            BarBackgroundColor = (Color)Application.Current.Resources["SecondColor"];
        }
    }
}