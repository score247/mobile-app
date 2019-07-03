using System;
using System.Threading;
using LiveScore.Core.Controls.MatchDetail;
using LiveScore.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(MatchDetailListView), typeof(MatchDetailListViewRenderer))]

namespace LiveScore.iOS.Renderers
{
    public class MatchDetailListViewRenderer : ListViewRenderer
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

        protected internal MatchDetailListView Source => Element as MatchDetailListView;

        private void TvDelegate_OnScrolled(object sender, EventArgs e)
        {
            var scrollY = Control.ContentOffset.Y;

            if (scrollY > 0)
            {
                MatchDetailListView.OnScrolling(scrollY);
            }

            if (scrollY <= 0)
            {
                MatchDetailListView.OnScrollingBack();
            }
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