namespace LiveScore.ViewModels
{
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using Common.Extensions;
    using Core.Factories;
    using Core.Services;
    using Core.ViewModels;
    using LiveScore.Services;
    using Prism.Commands;
    using Prism.Navigation;
    using Xamarin.Forms;

    public class MainViewModel : ViewModelBase
    {
        private Models.MenuItem selectedMenuItem;
        private bool nightMode;

        public MainViewModel(
            INavigationService navigationService,
            IGlobalFactory globalFactory,
            ISettingsService settingsService,
            IMenuService menuService) : base(navigationService, globalFactory, settingsService)
        {
            MenuItems = new ObservableCollection<Models.MenuItem>(menuService.GetAll());
            NavigateCommand = new DelegateAsyncCommand<string>(Navigate);
            NightMode = true;
        }

        public ObservableCollection<Models.MenuItem> MenuItems { get; set; }

        public Models.MenuItem SelectedMenuItem
        {
            get => selectedMenuItem;
            set => SetProperty(ref selectedMenuItem, value);
        }

        public bool NightMode
        {
            get { return nightMode; }
            set { SetProperty(ref nightMode, value); }
        }

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