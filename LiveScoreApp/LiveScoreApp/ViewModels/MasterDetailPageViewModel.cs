namespace LiveScoreApp.ViewModels
{
    using LiveScoreApp.Services;
    using Prism.Commands;
    using Prism.Navigation;
    using System.Collections.ObjectModel;
    using Xamarin.Forms;

    public class MasterDetailPageViewModel : ViewModelBase
    {
        private Models.MenuItem selectedMenuItem;

        public ObservableCollection<Models.MenuItem> MenuItems { get; set; }

        public Models.MenuItem SelectedMenuItem
        {
            get => selectedMenuItem;
            set => SetProperty(ref selectedMenuItem, value);
        }

        public DelegateCommand<string> NavigateCommand { get; set; }

        public MasterDetailPageViewModel(INavigationService navigationService, IMenuService menuService) : base(navigationService)
        {
            MenuItems = new ObservableCollection<Models.MenuItem>(menuService.GetAll());
            NavigateCommand = new DelegateCommand<string>(Navigate);
        }

        private async void Navigate(string page)
        {
            await NavigationService.NavigateAsync(nameof(NavigationPage) + "/" + page, useModalNavigation: true);
        }
    }
}