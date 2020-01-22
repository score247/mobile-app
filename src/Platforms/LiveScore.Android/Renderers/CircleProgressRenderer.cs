using Android.Animation;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Support.V4.Graphics.Drawable;
using Android.Text;
using Android.Views.Animations;
using Android.Widget;
using LiveScore.Core.Controls.CircleProgressBar;
using LiveScore.Droid.Renderers;
using Xamarin.Forms.Platform.Android;

[assembly: Xamarin.Forms.ExportRenderer(typeof(CircularProgress), typeof(CircleProgressRenderer))]

namespace LiveScore.Droid.Renderers
{
    public class CircleProgressRenderer : ViewRenderer<CircularProgress, ProgressBar>
    {
        private ProgressBar pBar;
        private Drawable pBarBackDrawable;
        private Drawable pBarForeDrawable;

        public CircleProgressRenderer(Context context) : base(context)
        {
            SetWillNotDraw(false);
        }

        public static void InitRender()
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<CircularProgress> e)
        {
            base.OnElementChanged(e);
            if (Control == null)
            {
                pBar = CreateNativeControl();
                SetNativeControl(pBar);
            }
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            pBar = CreateNativeControl();
            SetNativeControl(pBar);
            CreateAnimation();
        }

        protected override ProgressBar CreateNativeControl()
        {
            pBarBackDrawable = DrawableCompat.Wrap(Context.GetDrawable("CircularProgress_background"));
            pBarForeDrawable = DrawableCompat.Wrap(Context.GetDrawable("CircularProgress_drawable"));

            DrawableCompat.SetTint(pBarBackDrawable, Element.BackColor.ToAndroid());
            DrawableCompat.SetTint(pBarForeDrawable, Element.ForeColor.ToAndroid());

            var nativeControl = new ProgressBar(Context, null, global::Android.Resource.Attribute.ProgressBarStyleHorizontal)
            {
                Indeterminate = false,
                Max = Element.Maximum,
                ProgressDrawable = pBarForeDrawable,
                Rotation = -90f,
                LayoutParameters = new LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent),
            };

            nativeControl.SetBackground(pBarBackDrawable);

            return nativeControl;
        }

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);

            Rect bounds = new Rect();
            TextPaint paint = new TextPaint
            {
                Color = Element.TextColor.ToAndroid(),
                TextSize = Element.TextSize * 2
            };
            paint.GetTextBounds(Element.Text, 0, Element.Text.Length, bounds);
            if ((Width / 2) < bounds.Width())
            {
                float ratio = (float)(Width / 2) / bounds.Width();
                paint.TextSize *= ratio;
                paint.GetTextBounds(Element.Text, 0, Element.Text.Length, bounds);
            }

            int x = Width / 2 - bounds.CenterX();
            int y = Height / 2 - bounds.CenterY();
            canvas.DrawText(Element.Text, x, y, paint);
        }

        private void CreateAnimation()
        {
            ObjectAnimator anim = ObjectAnimator.OfInt(pBar, "Progress", 0, Element.Value);
            anim.SetDuration(Element.AnimationDuration);
            anim.SetInterpolator(new LinearInterpolator());
            anim.Start();
        }
    }
}