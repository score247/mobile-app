using LiveScore.Core.Controls.TabStrip.EventArgs;

namespace LiveScore.Core.Controls.TabStrip
{
    using Xamarin.Forms;

    public class TabItemListView : ListView
    {
        public TabItemListView() : base(ListViewCachingStrategy.RecycleElement)
        {
            RefreshControlColor = Color.White;
            Scrolled += TabItemListView_Scrolled;
        }

        private static void TabItemListView_Scrolled(object sender, ScrolledEventArgs e)
        {
            MessagingCenter.Send(
                nameof(TabStrip),
                TabItemScrollingEventArgs.EventName,
                new TabItemScrollingEventArgs(e.ScrollY > 0 ? e.ScrollY : 0));
        }

        public static void OnScrolling(double offset)
        {
            //MessagingCenter.Send(
            //    nameof(TabStrip),
            //    TabItemScrollingEventArgs.EventName,
            //    new TabItemScrollingEventArgs(offset));
        }
    }
}