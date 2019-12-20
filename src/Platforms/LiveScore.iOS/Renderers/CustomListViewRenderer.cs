using LiveScore.Core.Controls.CustomListView;
using LiveScore.iOS.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomListView), typeof(CustomListViewRenderer))]

namespace LiveScore.iOS.Renderers
{
    public class CustomListViewRenderer : ListViewRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<ListView> e)
        {
            base.OnElementChanged(e);

            if (Element is CustomListView listView)
            {
                listView.ScrollToTopEvent += ListView_ScrollToTop;
            }
        }

        private void ListView_ScrollToTop(object sender, System.EventArgs e)
        {
            Control.ScrollRectToVisible(new CoreGraphics.CGRect(0, 0, 1, 1), true);
        }
    }
}