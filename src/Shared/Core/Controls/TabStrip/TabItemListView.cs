using LiveScore.Core.Controls.TabStrip.EventArgs;

namespace LiveScore.Core.Controls.TabStrip
{
    using Xamarin.Forms;

    public class TabITemListView : ListView
    {
        public TabITemListView() : base(ListViewCachingStrategy.RecycleElement)
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