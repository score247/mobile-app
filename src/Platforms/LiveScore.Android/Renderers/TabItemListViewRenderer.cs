using Android.Content;
using Android.Widget;
using LiveScore.Core.Controls.TabStrip;
using LiveScore.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(TabItemListView), typeof(TabItemListViewRenderer))]

namespace LiveScore.Droid.Renderers
{
    public class TabItemListViewRenderer : ListViewRenderer
    {
        public TabItemListViewRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.ListView> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.ScrollStateChanged += Control_ScrollStateChanged;
            }
        }

        private void Control_ScrollStateChanged(object sender, AbsListView.ScrollStateChangedEventArgs e)
        {
        }
    }
}