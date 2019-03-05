namespace LiveScoreApp.Views
{
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuTabbedPage : TabbedPage
    {
        public MenuTabbedPage()
        {
#if DEBUG
            LiveReload.Init();
#endif
            InitializeComponent();
        }
    }
}