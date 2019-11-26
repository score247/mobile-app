using LiveScore.Core.Controls.TabStrip.EventArgs;

namespace LiveScore.Core.Controls.TabStrip
{
    using Xamarin.Forms;

    public class TabItemListView : ListView
    {
        public TabItemListView() : base(ListViewCachingStrategy.RecycleElement)
        {
            RefreshControlColor = Color.White;
        }

        public static void OnScrolling(double offset)
        {
            MessagingCenter.Send(
                nameof(TabStrip),
                TabItemScrollingEventArgs.EventName,
                new TabItemScrollingEventArgs(offset));
        }
    }
}