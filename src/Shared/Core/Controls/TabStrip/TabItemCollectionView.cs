using LiveScore.Core.Controls.TabStrip.EventArgs;
using LiveScore.Core.Views.Templates;
using Xamarin.Forms;

namespace LiveScore.Core.Controls.TabStrip
{
    public class TabItemCollectionView : CollectionView
    {
        public TabItemCollectionView()
        {
            var footerTemplate = new StackLayout { BackgroundColor = Color.Transparent, HeightRequest = 150 };
            footerTemplate.Children.Add(new Label { Text = " " });

            Footer = footerTemplate;
            GroupFooterTemplate = new EmptyDataTemplate();
        }

        protected override void OnScrolled(ItemsViewScrolledEventArgs e)
        {
            MessagingCenter.Send(
                nameof(TabStrip),
                TabItemScrollingEventArgs.EventName,
                new TabItemScrollingEventArgs(e.VerticalOffset > 0 ? e.VerticalOffset : 0));
        }
    }
}