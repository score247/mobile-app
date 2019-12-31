using LiveScore.Core.Controls.TabStrip.EventArgs;

namespace LiveScore.Core.Controls.TabStrip
{
    using Xamarin.Forms;

    public class TabItemListView : ListView
    {
        public TabItemListView() : base(ListViewCachingStrategy.RecycleElement)
        {
            Init();
        }

        public TabItemListView(ListViewCachingStrategy cachingStrategy) : base(cachingStrategy)
        {
            Init();
        }

        private void Init()
        {
            RefreshControlColor = Color.White;
            HasUnevenRows = true;
            SeparatorVisibility = SeparatorVisibility.None;
            var footerTemplate = new StackLayout { BackgroundColor = Color.Transparent, HeightRequest = 150 };
            footerTemplate.Children.Add(new Label { Text = " " });
            Footer = footerTemplate;
            Scrolled += TabItemListView_Scrolled;
        }

        private static void TabItemListView_Scrolled(object sender, ScrolledEventArgs e)
        {
            MessagingCenter.Send(
                nameof(TabStrip),
                TabItemScrollingEventArgs.EventName,
                new TabItemScrollingEventArgs(e.ScrollY > 0 ? e.ScrollY : 0));
        }
    }
}