using Xamarin.Forms;

namespace LiveScore.Core.Controls.CircleProgressBar
{
    public class CircularProgress : BoxView
    {
        public static readonly BindableProperty MaximumProperty = BindableProperty.Create(nameof(Maximum), typeof(int), typeof(CircularProgress), default(int));
        public static readonly BindableProperty ValueProperty = BindableProperty.Create(nameof(Value), typeof(int), typeof(CircularProgress), default(int));
        public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(CircularProgress), string.Empty);

        private readonly BindableProperty BackColorProperty = BindableProperty.Create(nameof(BackColor), typeof(Color), typeof(CircularProgress), Color.Transparent);
        private readonly BindableProperty ForeColorProperty = BindableProperty.Create(nameof(ForeColor), typeof(Color), typeof(CircularProgress), Color.Transparent);
        private readonly BindableProperty BarHeightProperty = BindableProperty.Create(nameof(BarHeight), typeof(double), typeof(CircularProgress), default(double));
        private readonly BindableProperty AnimationDurationProperty = BindableProperty.Create(nameof(AnimationDuration), typeof(int), typeof(CircularProgress), default(int));
        private readonly BindableProperty TextSizeProperty = BindableProperty.Create(nameof(TextSize), typeof(int), typeof(CircularProgress), default(int));
        private readonly BindableProperty TextColorProperty = BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(CircularProgress), Color.Black);


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

        public int Maximum
        {
            get { return (int)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
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