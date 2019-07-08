using System;
using LiveScore.Core.Controls.TabStrip;
using LiveScore.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(TabITemListView), typeof(TabItemListViewRenderer))]

namespace LiveScore.iOS.Renderers
{
    public class TabItemListViewRenderer : ListViewRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<ListView> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.MultipleTouchEnabled = true;
                var tvDelegate = new TableViewDelegate();
                Control.Delegate = tvDelegate;
                tvDelegate.OnScrolled += TvDelegate_OnScrolled;
            }
        }

        protected internal TabITemListView Source => Element as TabITemListView;

        private void TvDelegate_OnScrolled(object sender, EventArgs e)
        {
            var scrollY = Control.ContentOffset.Y;
            TabITemListView.OnScrolling(scrollY > 0 ? scrollY : 0);
        }
    }

    public class TableViewDelegate : UITableViewDelegate
    {
        public event EventHandler OnScrolled;

        public override void Scrolled(UIScrollView scrollView)
        {
            OnScrolled?.Invoke(scrollView, EventArgs.Empty);
        }
    }
}