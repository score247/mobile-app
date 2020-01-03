using CoreGraphics;
using LiveScore.iOS.Renderers;
using LiveScore.Soccer.Views.Matches.Templates.MatchDetails.Statistics;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(PossessionProgressBar), typeof(PossessionProgressBarRenderer))]

namespace LiveScore.iOS.Renderers
{
    public class PossessionProgressBarRenderer : ProgressBarRenderer
    {
        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            const float X = 1.0f;
            const float Y = 5.0f; // This changes the height

            CGAffineTransform transform = CGAffineTransform.MakeScale(X, Y);
            Control.Transform = transform;
            Control.ClipsToBounds = true;
            Control.Layer.MasksToBounds = true;
        }
    }
}