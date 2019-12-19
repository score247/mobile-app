using LiveScore.Core.Controls.Calendar;
using LiveScore.iOS.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CalendarListView), typeof(CalendarDisableScrollListViewRenderer))]

namespace LiveScore.iOS.Renderers
{
    public class CalendarDisableScrollListViewRenderer : ListViewRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<ListView> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                // Unsubscribe
            }

            if (e.NewElement != null)
            {
                Control.ScrollEnabled = false;
            }
        }
    }
}