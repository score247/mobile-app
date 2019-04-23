namespace LiveScore.Core.Views.Templates
{
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NavigationTitleTemplate : ContentView
    {
        public NavigationTitleTemplate()
        {
            InitializeComponent();
            titleLabel.BindingContext = this;
        }

        public static readonly BindableProperty TitleProperty
            = BindableProperty.Create("Title", typeof(string), typeof(NavigationTitleTemplate), string.Empty);

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
    }
}