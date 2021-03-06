using LiveScore.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(WebView), typeof(TransparentWebViewRenderer))]

namespace LiveScore.iOS.Renderers
{
    public class TransparentWebViewRenderer : WkWebViewRenderer
    {
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            Opaque = false;
            BackgroundColor = UIColor.Clear;
        }
    }
}