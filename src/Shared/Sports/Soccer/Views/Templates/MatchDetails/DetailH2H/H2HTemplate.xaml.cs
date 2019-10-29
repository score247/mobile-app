using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LiveScore.Soccer.Views.Templates.MatchDetails.DetailH2H
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class H2HTemplate : DataTemplate
    {
        public H2HTemplate()
        {
            InitializeComponent();
        }

#pragma warning disable S2325 // Methods and properties that don't access instance data should be static

        public void H2HMatches_OnScrolled(object sender, ItemsViewScrolledEventArgs e)
        {
            MessagingCenter.Send("TabListView", "OnScrolling", e.VerticalOffset > 0 ? e.VerticalOffset : 0);
        }

#pragma warning restore S2325 // Methods and properties that don't access instance data should be static
    }
}