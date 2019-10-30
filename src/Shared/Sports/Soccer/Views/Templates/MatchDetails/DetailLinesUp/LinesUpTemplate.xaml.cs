using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LiveScore.Soccer.Views.Templates.DetailLinesUp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LinesUpTemplate : DataTemplate
    {
        public LinesUpTemplate()
        {
            InitializeComponent();
        }

        private void ItemsView_OnScrolled(object sender, ItemsViewScrolledEventArgs e)
        {
            MessagingCenter.Send("TabListView", "OnScrolling", e.VerticalOffset > 0 ? e.VerticalOffset : 0);
        }
    }
}