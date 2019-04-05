namespace LiveScore.Views
{
    using Prism.Navigation;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainView : IMasterDetailPageOptions
    {
        public MainView()
        {
            InitializeComponent();
        }

        public bool IsPresentedAfterNavigation => Device.Idiom != TargetIdiom.Phone;
    }
}