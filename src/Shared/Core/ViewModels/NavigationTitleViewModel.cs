namespace LiveScore.Core.ViewModels
{
    using System.Threading.Tasks;
    using Common.Extensions;
    using LiveScore.Core;
    using LiveScore.Core.Views;
    using Prism.Navigation;

    public class NavigationTitleViewModel : ViewModelBase
    {
        public NavigationTitleViewModel(INavigationService navigationService, IDependencyResolver serviceLocator)
            : base(navigationService, serviceLocator)
        {
            SelectSportCommand = new DelegateAsyncCommand(NavigateSelectSportPage);
            CurrentSportName = SettingsService.CurrentSportType.DisplayName;
        }

        public string CurrentSportName { get; }

        public DelegateAsyncCommand SelectSportCommand { get; }

        private async Task NavigateSelectSportPage()
            => await NavigationService.NavigateAsync("NavigationPage/" + nameof(SelectSportView), useModalNavigation: true);
    }
}