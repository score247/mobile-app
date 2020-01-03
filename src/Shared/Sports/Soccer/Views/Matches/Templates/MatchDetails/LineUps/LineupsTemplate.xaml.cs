using LiveScore.Core.Controls.TabStrip;
using LiveScore.Core.Controls.TabStrip.EventArgs;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LiveScore.Soccer.Views.Matches.Templates.MatchDetails.LineUps
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LineUpsTemplate : DataTemplate
    {
        public LineUpsTemplate()
        {
            InitializeComponent();
        }

        public static void TabItemCollectionView_Scrolled(object sender, ItemsViewScrolledEventArgs e)
        {
            MessagingCenter.Send(
               nameof(TabStrip),
               TabItemScrollingEventArgs.EventName,
               new TabItemScrollingEventArgs(e.VerticalOffset > 200 ? e.VerticalOffset : 0));
        }
    }
}