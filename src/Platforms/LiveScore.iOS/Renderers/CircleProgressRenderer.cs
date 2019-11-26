using System;
using System.ComponentModel;
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
        private static readonly CAMediaTimingFunction AnimationTimingFunc = CAMediaTimingFunction.FromName(CAMediaTimingFunction.EaseOut);

        private CAShapeLayer backgroundCircle;
        private CAShapeLayer indicatorCircle;
        private UILabel indicatorLabel;
        private int indicatorFontSize;

        private bool isSizeChanged;
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

            CreateBackgroundCircle();

            CreateIndicatorCircle();

            CreateIndicatorLabel();
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == nameof(CircularProgress.Value)
                || e.PropertyName == nameof(CircularProgress.Maximum))
            {
                ProgressValue = CalculateValue();
            }

            isSizeChanged |= (e.PropertyName == nameof(CircularProgress.Width)
                || e.PropertyName == nameof(CircularProgress.Height)
                || e.PropertyName == nameof(CircularProgress.BarHeight));

            if (e.PropertyName == nameof(CircularProgress.Text))
            {
                UpdateIndicatorLabelSize();
            }

        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            CreatePathAndUpdateRadius();

            UpdateIndicatorLabelSize();
            AddSubview(indicatorLabel);

            Animate();
        }

        private void UpdateIndicatorLabelSize()
        {
            if (indicatorFontSize == 0)
            {
                return;
            }

            indicatorLabel.Text = string.IsNullOrEmpty(Element.Text) ? "  " : Element.Text;

            indicatorLabel.Font = UIFont.SystemFontOfSize(indicatorFontSize);
            indicatorLabel.InvalidateIntrinsicContentSize();

            indicatorLabel.Frame = new CGRect(
                    (Frame.Width / 2) - (indicatorLabel.IntrinsicContentSize.Width / 2),
                    (Frame.Height / 2) - (indicatorLabel.IntrinsicContentSize.Height / 2),
                    indicatorLabel.IntrinsicContentSize.Width,
                    indicatorLabel.IntrinsicContentSize.Height);
        }

        private double CalculateValue() => (double)Element.Value / Element.Maximum;

        private void CreateIndicatorLabel()
        {

            indicatorLabel = CreateUILabel();

            UpdateIndicatorLabelSize();
        }

        private UILabel CreateUILabel()
        => new UILabel
        {
            AdjustsFontSizeToFitWidth = true,
            TextAlignment = UITextAlignment.Center,
            Font = UIFont.SystemFontOfSize(indicatorFontSize),
            TextColor = Element.TextColor.ToUIColor(),
        };

        private void CreateIndicatorCircle()
        {
            indicatorCircle = CreateCAShapeLayer(Element.ForeColor.ToCGColor(), true);

            Layer.AddSublayer(indicatorCircle);
        }

        private void CreateBackgroundCircle()
        {
            backgroundCircle = CreateCAShapeLayer(Element.BackColor.ToCGColor());

            Layer.AddSublayer(backgroundCircle);
        }

        private CAShapeLayer CreateCAShapeLayer(CGColor strokeColor, bool hasLineCap = false)
        => new CAShapeLayer
        {
            StrokeColor = strokeColor,
            FillColor = UIColor.Clear.CGColor,
            LineWidth = new nfloat(Element.BarHeight),
            Frame = Bounds,
            LineCap = hasLineCap ? CAShapeLayer.CapButt : CAShapeLayer.CapRound
        };

        private void CreatePathAndUpdateRadius()
        {
            var radius = CalculateRadius(new nfloat(Element.BarHeight));

            if (radius > 0)
            {
                var circlePath = CreateCirclePath(radius);

                backgroundCircle.Path = circlePath.CGPath;
                backgroundCircle.StrokeEnd = new nfloat(1.0);

                indicatorCircle.Path = circlePath.CGPath;
                indicatorCircle.StrokeEnd = new nfloat(ProgressValue);
            }
        }

        private nfloat CalculateRadius(nfloat lineWidth)
        {
            var radius = new nfloat(0);

            if (isSizeChanged)
            {
                isSizeChanged = false;

                var size = (float)Math.Min(Frame.Size.Width, Frame.Size.Height);

                radius = (size - (float)lineWidth - 2) / 2f;
            }

            return radius;
        }

        private UIBezierPath CreateCirclePath(double radius)
        {
            var circlePath = new UIBezierPath();
            circlePath.AddArc(new CGPoint(Frame.Size.Width / 2, Frame.Size.Height / 2), (nfloat)radius, (nfloat)startAngle, (nfloat)(startAngle + 2 * Math.PI), true);

            return circlePath;
        }

        private void Animate()
        {
            indicatorCircle.StrokeStart = new nfloat(Start);
            indicatorCircle.StrokeEnd = new nfloat(ProgressValue);

            indicatorCircle.AddAnimation(CreateAnimation(), "appear");
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