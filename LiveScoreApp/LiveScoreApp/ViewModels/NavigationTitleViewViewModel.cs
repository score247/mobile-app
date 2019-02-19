namespace LiveScoreApp.ViewModels
{
    using LiveScoreApp.Models;
    using LiveScoreApp.Services;
    using Prism.Commands;
    using Prism.Navigation;
    using System.Collections.ObjectModel;

    public class NavigationTitleViewViewModel : ViewModelBase
    {
        private string title;

        public string SubTitle
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        public DelegateCommand SelectSportCommand { get; set; }

        public NavigationTitleViewViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            SubTitle = "Soccer";
            SelectSportCommand = new DelegateCommand(NavigateSelectSportPage);
        }

        private async void NavigateSelectSportPage()
        {
            await NavigationService.NavigateAsync("NavigationPage/SelectSportPage");
        }
    }
}