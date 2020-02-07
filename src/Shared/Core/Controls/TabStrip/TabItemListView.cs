using LiveScore.Core.Controls.TabStrip.EventArgs;
using Xamarin.Forms;

namespace LiveScore.Core.Controls.TabStrip
{

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

        public void OnScroll(double scrollY)
        {
            MessagingCenter.Send(
                nameof(TabStrip),
                TabItemScrollingEventArgs.EventName,
                new TabItemScrollingEventArgs(scrollY > 0 ? scrollY : 0));
        }

        private void Init()
        {
            HasUnevenRows = true;
            SeparatorVisibility = SeparatorVisibility.None;
            var footerTemplate = new StackLayout { BackgroundColor = Color.Transparent, HeightRequest = 150 };
            footerTemplate.Children.Add(new Label { Text = " " });
            SelectionMode = ListViewSelectionMode.None;
            Footer = footerTemplate;
            Scrolled += TabItemListView_Scrolled;
        }

        private void TabItemListView_Scrolled(object sender, ScrolledEventArgs e)
        {
            OnScroll(e.ScrollY);
        }
    }
}