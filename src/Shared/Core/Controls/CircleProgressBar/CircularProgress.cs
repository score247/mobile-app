using Xamarin.Forms;

namespace LiveScore.Core.Controls.CircleProgressBar
{
    public class CircularProgress : BoxView
    {
        public readonly BindableProperty BackColorProperty = BindableProperty.Create(nameof(BackColor), typeof(Color), typeof(CircularProgress), Color.Transparent);
        public readonly BindableProperty ForeColorProperty = BindableProperty.Create(nameof(ForeColor), typeof(Color), typeof(CircularProgress), Color.Transparent);
        public readonly BindableProperty BarHeightProperty = BindableProperty.Create(nameof(BarHeight), typeof(double), typeof(CircularProgress), default(double));
        public readonly BindableProperty MinimunProperty = BindableProperty.Create(nameof(Minimun), typeof(int), typeof(CircularProgress), default(int));
        public readonly BindableProperty MaximunProperty = BindableProperty.Create(nameof(Maximun), typeof(int), typeof(CircularProgress), default(int));
        public readonly BindableProperty ValueProperty = BindableProperty.Create(nameof(Value), typeof(int), typeof(CircularProgress), default(int));
        public readonly BindableProperty AnimationDurationProperty = BindableProperty.Create(nameof(AnimationDuration), typeof(int), typeof(CircularProgress), default(int));
        public readonly BindableProperty TextSizeProperty = BindableProperty.Create(nameof(TextSize), typeof(int), typeof(CircularProgress), default(int));
        public readonly BindableProperty TextMarginProperty = BindableProperty.Create(nameof(TextMargin), typeof(int), typeof(CircularProgress), default(int));
        public readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(CircularProgress), string.Empty);
        public readonly BindableProperty TextColorProperty = BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(CircularProgress), Color.Black);
              
        public Color BackColor
        {
            get { return (Color)GetValue(BackColorProperty); }
            set { SetValue(BackColorProperty, value); }
        }

        public Color ForeColor
        {
            get { return (Color)GetValue(ForeColorProperty); }
            set { SetValue(ForeColorProperty, value); }
        }

        public double BarHeight
        {
            get { return (double)GetValue(BarHeightProperty); }
            set { SetValue(BarHeightProperty, value); }
        }

        public int Minimun
        {
            get { return (int)GetValue(MinimunProperty); }
            set { SetValue(MinimunProperty, value); }
        }

        public int Maximun
        {
            get { return (int)GetValue(MaximunProperty); }
            set { SetValue(MaximunProperty, value); }
        }

        public int Value
        {
            get { return (int)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public int AnimationDuration
        {
            get { return (int)GetValue(AnimationDurationProperty); }
            set { SetValue(AnimationDurationProperty, value); }
        }

        public int TextSize
        {
            get { return (int)GetValue(TextSizeProperty); }
            set { SetValue(TextSizeProperty, value); }
        }

        public int TextMargin
        {
            get { return (int)GetValue(TextMarginProperty); }
            set { SetValue(TextMarginProperty, value); }
        }

        public string Text
        {
            get { return GetValue(TextProperty).ToString(); }
            set { SetValue(TextProperty, value); }
        }

        public Color TextColor
        {
            get { return (Color)GetValue(TextColorProperty); }
            set { SetValue(TextColorProperty, value); }
        }
    }
}
