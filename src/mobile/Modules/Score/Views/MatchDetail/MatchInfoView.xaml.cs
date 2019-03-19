namespace Score.Views.MatchDetail
{
    using Xamarin.Forms;

    public partial class MatchInfoView : ContentView
    {
        public static readonly BindableProperty FooProperty =
            BindableProperty.Create(nameof(Foo), typeof(string), typeof(MatchInfoView));

        public MatchInfoView()
        {
            InitializeComponent();
        }

        public string Foo
        {
            get => (string)GetValue(FooProperty);
            set => SetValue(FooProperty, value);
        }
    }
}
