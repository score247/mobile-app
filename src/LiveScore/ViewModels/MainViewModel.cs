namespace LiveScore.ViewModels
{
    using System.Threading.Tasks;
    using Common.Extensions;
    using Core.Factories;
    using Core.Services;
    using Core.ViewModels;
    using Prism.Commands;
    using Prism.Navigation;
    using Xamarin.Forms;

    public class MainViewModel : ViewModelBase
    {
        public MainViewModel(
            INavigationService navigationService,
            IGlobalFactoryProvider globalFactory,
            ISettingsService settingsService) : base(navigationService, globalFactory, settingsService)
        {
            NavigateCommand = new DelegateAsyncCommand<string>(Navigate);
            NightMode = true;
        }

        public bool NightMode { get; set; }

        public DelegateAsyncCommand<string> NavigateCommand { get; set; }

        public DelegateCommand ChangeThemeCommand => new DelegateCommand(OnChangeThemeExecuted);

        private void OnChangeThemeExecuted()
        {
            // TODO Implement later
        }

        private async Task Navigate(string page)
        {
            await NavigationService.NavigateAsync(nameof(NavigationPage) + "/" + page, useModalNavigation: true);
        }
    }
}