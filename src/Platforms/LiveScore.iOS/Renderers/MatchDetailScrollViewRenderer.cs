using LiveScore.Core.Controls.MatchDetail;
using LiveScore.iOS.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(MatchDetailScrollView), typeof(MatchDetailScrollViewRenderer))]

namespace LiveScore.iOS.Renderers
{
    public class MatchDetailScrollViewRenderer : ScrollViewRenderer
    {
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            ScrollEnabled = false;
        }
    }
}