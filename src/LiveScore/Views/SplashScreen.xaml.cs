using System.Threading.Tasks;
using Prism.Navigation;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LiveScore.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SplashScreen : ContentPage
    {
        public SplashScreen(INavigationService NavigationService)
        {
            InitializeComponent();

            Xamarin.Forms.Device.BeginInvokeOnMainThread(async () =>
            {
                await Task.Delay(2000);
                await NavigationService.NavigateAsync(nameof(MainView) + "/" + nameof(MenuTabbedView)).ConfigureAwait(true);
            });
        }
    }
}