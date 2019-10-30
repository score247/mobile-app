using LiveScore.Core.Controls.TabStrip.EventArgs;
using Xamarin.Forms;

namespace LiveScore.Core.Controls.TabStrip
{
    public class TabItemCollectionView : CollectionView
    {
        protected override void OnScrolled(ItemsViewScrolledEventArgs e)
        {
            MessagingCenter.Send(
                nameof(TabStrip),
                TabItemScrollingEventArgs.EventName,
                new TabItemScrollingEventArgs(e.VerticalOffset > 0 ? e.VerticalOffset : 0));
        }
    }
}