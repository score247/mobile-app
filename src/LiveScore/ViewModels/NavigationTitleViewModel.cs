namespace LiveScore.ViewModels
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Common.Extensions;
    using Core.ViewModels;
    using LiveScore.Core;
    using LiveScore.Models;
    using LiveScore.Services;
    using LiveScore.Views;
    using Prism.Navigation;

    public class NavigationTitleViewModel : ViewModelBase
    {
        private readonly ISportService sportService;
        private IEnumerable<SportItem> sportItems;

        public NavigationTitleViewModel(INavigationService navigationService, IDepdendencyResolver serviceLocator, ISportService sportService)
            : base(navigationService, serviceLocator)
        {
            this.sportService = sportService;
            SelectSportCommand = new DelegateAsyncCommand(NavigateSelectSportPage);
        }

        public string CurrentSportName { get; set; }

        public DelegateAsyncCommand SelectSportCommand { get; }

        public override void OnAppearing()
        {
            CurrentSportName = SettingsService.CurrentSportType.DisplayName;
            sportItems = sportService.GetSportItems();

            foreach (var sportItem in sportItems)
            {
                sportItem.IsVisible = sportItem.Type.Value == SettingsService.CurrentSportType.Value;
            }
        }

        private async Task NavigateSelectSportPage()
        {
            var parameters = new NavigationParameters
            {
                { nameof(sportItems), sportItems }
            };

            await NavigationService.NavigateAsync("NavigationPage/" + nameof(SelectSportView), parameters, true);
        }
    }
}