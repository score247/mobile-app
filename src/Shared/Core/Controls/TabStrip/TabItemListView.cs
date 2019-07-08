namespace LiveScore.Core.Controls.TabStrip
{
    using Xamarin.Forms;

    public class TabITemListView : ListView
    {
        public delegate void OverScrollUpdateEvent(object sender, float offset);

        public TabITemListView() : base(ListViewCachingStrategy.RecycleElement)
        {
        }

        public static void OnScrolling(double offset)
        {
            MessagingCenter.Send("TabListView", "OnScrolling", offset);
        }
    }
}