using Android.Content;
using LiveScore.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using static Android.Text.TextUtils;

[assembly: ExportRenderer(typeof(Button), typeof(MiddleTruncateButtonRenderer))]

namespace LiveScore.Droid.Renderers
{
    public class MiddleTruncateButtonRenderer : ButtonRenderer
    {
        public MiddleTruncateButtonRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);
            Control.SetMaxLines(1);
            Control.Ellipsize = TruncateAt.Middle;
        }
    }
}