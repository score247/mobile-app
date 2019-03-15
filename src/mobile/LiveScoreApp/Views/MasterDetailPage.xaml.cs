namespace LiveScoreApp.Views
{
    using Prism.Navigation;

    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterDetailPage : IMasterDetailPageOptions
    {
        public MasterDetailPage()
        {
            InitializeComponent();
        }

        public bool IsPresentedAfterNavigation => Device.Idiom != TargetIdiom.Phone;
    }
}