namespace LiveScore.Core.Controls.CustomListView
{
    using Xamarin.Forms;

    public class CustomListView : ListView
    {
        public CustomListView() : base(ListViewCachingStrategy.RecycleElementAndDataTemplate)
        {
        }

#if  AUTOTEST

        public CustomListView(ListViewCachingStrategy cachingStrategy) : base(ListViewCachingStrategy.RetainElement)
        {
        }

#else

        public CustomListView(ListViewCachingStrategy cachingStrategy) : base(cachingStrategy)
        {
        }

#endif
    }
}