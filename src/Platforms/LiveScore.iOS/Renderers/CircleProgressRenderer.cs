using System;
using System.ComponentModel;
using System.Diagnostics;
using CoreAnimation;
using CoreGraphics;
using Foundation;
using LiveScore.Core.Controls.CircleProgressBar;
using UIKit;
using Xamarin.Forms.Platform.iOS;

[assembly: Xamarin.Forms.ExportRenderer(typeof(CircularProgress), typeof(LiveScore.iOS.Renderers.CircleProgressRenderer))]

namespace LiveScore.iOS.Renderers
{
    public class CircleProgressRenderer : VisualElementRenderer<CircularProgress>
    {
        private const string AnimationKeyPath = "strokeEnd";
        private const double Start = 0.0;
        private static CAMediaTimingFunction AnimationTimingFunc = CAMediaTimingFunction.FromName(CAMediaTimingFunction.EaseOut);

        private CAShapeLayer backgroundCircle;
        private CAShapeLayer indicatorCircle;
        private UILabel indicatorLabel;
        private CGSize indicatorLabelSize;
        private int indicatorFontSize;

        private bool isSizeChanged;
        private nfloat ProgressRadius;
        private double ProgressValue;

        private readonly double startAngle = 1.5 * Math.PI;

      
        protected override void OnElementChanged(ElementChangedEventArgs<CircularProgress> e)
        {
            base.OnElementChanged(e);

            if (Element == null)
            {
                return;
            }

            indicatorFontSize = Element.TextSize;

            backgroundCircle = new CAShapeLayer();

            CreateBackgroundCircle();

            CreateIndicatorCircle();

            CreateIndicatorLabel();
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == CircularProgress.ValueProperty.PropertyName
                || e.PropertyName == CircularProgress.MaximumProperty.PropertyName
                )
            {
                isSizeChanged = true;
                Debug.WriteLine("Need to update progress bar");

                ProgressValue = CalculateValue();
            }
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            double radius = CreatePathAndReturnRadius();

            double heightRatio = (radius - Element.TextMargin) / indicatorLabelSize.Height;
            double widthRatio = (radius - Element.TextMargin) / indicatorLabelSize.Width;
            double ratio = 1;

            if (heightRatio < widthRatio)
                ratio = (radius - Element.TextMargin) / indicatorLabelSize.Height;
            else
                ratio = (radius - Element.TextMargin) / indicatorLabelSize.Width;

            //TODO: fontSize after re-bind is bigger than initial
            indicatorFontSize = -16;

            //indicatorFontSize = (int)Math.Round(indicatorFontSize * ratio, 0, MidpointRounding.ToEven);
                        
            indicatorLabel.Text = Element.Text?.ToString();

            indicatorLabel.Font = UIFont.SystemFontOfSize(indicatorFontSize);
            indicatorLabel.InvalidateIntrinsicContentSize();
            indicatorLabelSize = indicatorLabel.IntrinsicContentSize;

            indicatorLabel.Frame = new CGRect((Frame.Width / 2) - (indicatorLabelSize.Width / 2), (Frame.Height / 2) - (indicatorLabelSize.Height / 2), indicatorLabelSize.Width, indicatorLabelSize.Height);

            Debug.WriteLine($"LayoutSubviews indicatorLabel.Frame width {indicatorLabel.Frame.Width} height {indicatorLabel.Frame.Height}");

            this.AddSubview(indicatorLabel);
            animate();

        }

        private double CalculateValue() => (double)Element.Value / Element.Maximum;

        private void CreateIndicatorLabel()
        {
            Debug.WriteLine($"CreateIndicatorLabel with text {Element.Text}");

            indicatorLabel = new UILabel();
            indicatorLabel.AdjustsFontSizeToFitWidth = true;
            indicatorLabel.Font = UIFont.SystemFontOfSize(indicatorFontSize);
            indicatorLabel.Text = string.IsNullOrWhiteSpace(Element.Text)? " " : Element.Text;
            indicatorLabel.TextColor = Element.TextColor.ToUIColor();
            indicatorLabel.TextAlignment = UITextAlignment.Center;
            indicatorLabelSize = indicatorLabel.IntrinsicContentSize;
        }

        private void CreateIndicatorCircle()
        {
            indicatorCircle = new CAShapeLayer();
            indicatorCircle.StrokeColor = Element.ForeColor.ToCGColor();
            indicatorCircle.FillColor = UIColor.Clear.CGColor;
            indicatorCircle.LineWidth = new nfloat(Element.BarHeight);
            indicatorCircle.Frame = this.Bounds;
            indicatorCircle.LineCap = CAShapeLayer.CapButt;
            this.Layer.AddSublayer(indicatorCircle);
        }

        private void CreateBackgroundCircle()
        {
            backgroundCircle.StrokeColor = Element.BackColor.ToCGColor();
            backgroundCircle.FillColor = UIColor.Clear.CGColor;
            backgroundCircle.LineWidth = new nfloat(Element.BarHeight);
            backgroundCircle.Frame = this.Bounds;
            this.Layer.AddSublayer(backgroundCircle);
        }

        private double CreatePathAndReturnRadius()
        {
            ProgressRadius = CalculateRadius(new nfloat(Element.BarHeight));

            var circlePath = CreateCirclePath(ProgressRadius);
            backgroundCircle.Path = circlePath.CGPath;
            indicatorCircle.Path = circlePath.CGPath;
            backgroundCircle.StrokeEnd = new nfloat(1.0);
            indicatorCircle.StrokeEnd = new nfloat(ProgressValue);

            return ProgressRadius;
        }

        private nfloat CalculateRadius(nfloat lineWidth)
        {
            if (isSizeChanged)
            {
                isSizeChanged = false;

                nfloat width = Frame.Size.Width;
                nfloat height = Frame.Size.Height;

                Debug.WriteLine($"CalculateRadius width {width} - height {height}");

                var size = (float)Math.Min(width, height);

                ProgressRadius = (size - (float)lineWidth - 2) / 2f;
            }

            return ProgressRadius;
        }

        private UIBezierPath CreateCirclePath(double radius)
        {
            var circlePath = new UIBezierPath();
            circlePath.AddArc(new CGPoint(Frame.Size.Width / 2, Frame.Size.Height / 2), (nfloat)radius, (nfloat)startAngle, (nfloat)(startAngle + 2 * Math.PI), true);

            return circlePath;
        }

        private void animate()
        {
            var animation = CreateAnimation();

            indicatorCircle.StrokeStart = new nfloat(Start);
            indicatorCircle.StrokeEnd = new nfloat(ProgressValue);
            indicatorCircle.AddAnimation(animation, "appear");
        }

        private CABasicAnimation CreateAnimation()
            => new CABasicAnimation
            {
                KeyPath = AnimationKeyPath,
                Duration = (double)Element.AnimationDuration / 1000,
                From = new NSNumber(Start),
                To = new NSNumber(ProgressValue),
                TimingFunction = AnimationTimingFunc
            };
       
    }
}