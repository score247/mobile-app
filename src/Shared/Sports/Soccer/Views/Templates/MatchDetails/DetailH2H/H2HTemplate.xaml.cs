using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LiveScore.Soccer.Views.Templates.DetailH2H
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class H2HTemplate : DataTemplate
    {
        public H2HTemplate()
        {
            InitializeComponent();
        }

        private static void H2HMatches_OnScrolled(object sender, ItemsViewScrolledEventArgs e)
        {
            MessagingCenter.Send("TabListView", "OnScrolling", e.VerticalOffset > 0 ? e.VerticalOffset : 0);
        }
    }
}