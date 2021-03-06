using System.Collections.Generic;
using System.Linq;
using CoreGraphics;
using LiveScore.Core.Controls.MatchesListView;
using LiveScore.iOS.Extensions;
using LiveScore.iOS.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(MatchesListView), typeof(MatchesListViewRenderer))]

namespace LiveScore.iOS.Renderers
{
    public class MatchesListViewRenderer : ListViewRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<ListView> e)
        {
            base.OnElementChanged(e);

            if (Element is MatchesListView listView)
            {
                listView.ScrollToTopEvent += ListView_ScrollToTop;
                listView.ScrollToFirstItemEvent += ListView_ScrollToFirstItemEvent;
                listView.AdjustFooterHeightEvent += ListView_AdjustFooterHeightEvent;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (Element is MatchesListView listView)
            {
                listView.ScrollToTopEvent -= ListView_ScrollToTop;
                listView.ScrollToFirstItemEvent -= ListView_ScrollToFirstItemEvent;
                listView.AdjustFooterHeightEvent -= ListView_AdjustFooterHeightEvent;
            }

            Control.DisposeEx();

            base.Dispose(disposing);
        }

        private void ListView_ScrollToTop(object sender, System.EventArgs e)
        {
            Control.ScrollRectToVisible(new CoreGraphics.CGRect(0, 0, 1, 1), true);
        }

        private void ListView_ScrollToFirstItemEvent(object sender, System.EventArgs e)
        {
            if (sender is MatchesListView listView)
            {
                var headerHeight = (Element.HeaderElement as View)?.Height ?? 0;
                listView.FooterHeight = 1000; // For showing skeleton loading in footer

                Control.SetContentOffset(new CGPoint(0, headerHeight), true);
            }
        }

        private void ListView_AdjustFooterHeightEvent(object sender, System.EventArgs e)
        {
            if (sender is MatchesListView listView)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    var groupHeight = (double)App.Current.Resources["FunctionGroupHeight"];
                    var groupCount = (listView.ItemsSource as IEnumerable<object>)?.Count() ?? 0;

                    if (groupCount == 0)
                    {
                        listView.FooterHeight = listView.Height - Control.EstimatedRowHeight;
                        return;
                    }

                    var estimatedFooterHeight = listView.Height -
                                                Control.EstimatedRowHeight * Control.VisibleCells.Length -
                                                groupCount * groupHeight;
                    listView.FooterHeight = estimatedFooterHeight > 0 ? estimatedFooterHeight : 1;
                });
            }
        }
    }
}