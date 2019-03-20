using Xamarin.Forms;

namespace Common.Views.Soccer
{
    public partial class MatchView : ContentView
    {
        public static readonly BindableProperty FooProperty =
            BindableProperty.Create("Foo", typeof(string), typeof(MatchView), string.Empty);

        public MatchView()
        {
            InitializeComponent();
            label.BindingContext = this;
            label.SetBinding(Label.TextProperty, "Foo");
        }

        public string Foo
        {
            get => (string)GetValue(FooProperty);
            set => SetValue(FooProperty, value);
        }
    }
}
