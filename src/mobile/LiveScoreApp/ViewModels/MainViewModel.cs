namespace LiveScoreApp.ViewModels
{
    using System.Collections.ObjectModel;
    using Common.ViewModels;
    using LiveScoreApp.Services;
    using LiveScoreApp.ViewResources;
    using Prism.Commands;
    using Prism.Navigation;
    using Xamarin.Forms;

    public class MainViewModel : ViewModelBase
    {
        private Models.MenuItem selectedMenuItem;
        private bool nightMode;

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

        public DelegateCommand<string> NavigateCommand { get; set; }

        public DelegateCommand ChangeThemeCommand => new DelegateCommand(OnChangeThemeExecuted);

        private void OnChangeThemeExecuted()
        {
            var currentResource = App.Current.Resources;
            currentResource.MergedDictionaries.Clear();

            if (NightMode)
            {
                currentResource.MergedDictionaries.Add(new DarkThemeResource());
            }
            else
            {
                currentResource.MergedDictionaries.Add(new LightThemeResource());
            }
        }

        public MainViewModel(INavigationService navigationService, IMenuService menuService) : base(navigationService)
        {
            MenuItems = new ObservableCollection<Models.MenuItem>(menuService.GetAll());
            NavigateCommand = new DelegateCommand<string>(Navigate);
            NightMode = true;
        }

        private async void Navigate(string page)
        {
            await NavigationService.NavigateAsync(nameof(NavigationPage) + "/" + page, useModalNavigation: true);
        }
    }
}