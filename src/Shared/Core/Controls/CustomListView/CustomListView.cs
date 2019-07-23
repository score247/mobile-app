namespace LiveScore.Core.Controls.CustomListView
{
    using Xamarin.Forms;

    public class CustomListView : ListView
    {
#if TEST || AUTOTEST || DEBUG

        public CustomListView() : base(ListViewCachingStrategy.RetainElement)
        {
        }

#else

        public CustomListView() : base(ListViewCachingStrategy.RecycleElement)
        {
        }

#endif
    }
}