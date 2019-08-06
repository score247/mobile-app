using LiveScore.Core.Controls;
using LiveScore.iOS.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(DetailScrollView), typeof(DetailScrollViewRenderer))]
namespace LiveScore.iOS.Renderers
{
    public class DetailScrollViewRenderer : ScrollViewRenderer
    {
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            ScrollEnabled = false;
        }
    }
}