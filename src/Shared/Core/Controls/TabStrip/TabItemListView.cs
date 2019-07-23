namespace LiveScore.Core.Controls.TabStrip
{
    using Xamarin.Forms;

    public class TabITemListView : ListView
    {
#if TEST || AUTOTEST

          public TabITemListView() : base(ListViewCachingStrategy.RetainElement)
        {
        }

#else

        public TabITemListView() : base(ListViewCachingStrategy.RecycleElement)
        {
        }

#endif

        public static void OnScrolling(double offset)
        {
            MessagingCenter.Send("TabListView", "OnScrolling", offset);
        }
    }
}