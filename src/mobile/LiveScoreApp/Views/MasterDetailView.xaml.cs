namespace LiveScoreApp.Views
{
    using Prism.Navigation;

    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterDetailView : IMasterDetailPageOptions
    {
        public MasterDetailView()
        {
            InitializeComponent();
        }

        public bool IsPresentedAfterNavigation => Device.Idiom != TargetIdiom.Phone;
    }
}