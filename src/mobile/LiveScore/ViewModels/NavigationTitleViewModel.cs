namespace LiveScore.ViewModels
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Common.Extensions;
    using Core.ViewModels;
    using Core.Services;
    using LiveScore.Models;
    using LiveScore.Services;
    using LiveScore.Views;
    using Prism.Navigation;
    using Core.Factories;

    public class NavigationTitleViewModel : ViewModelBase
    {
        private readonly ISportService sportService;
        private string currentSportName;
        private IEnumerable<SportItem> sportItems;

        public NavigationTitleViewModel(
            INavigationService navigationService,
            IGlobalFactory globalFactory,
            ISettingsService settingsService,
            ISportService sportService) : base(navigationService, globalFactory, settingsService)
        {
            this.sportService = sportService;
            SelectSportCommand = new DelegateAsyncCommand(NavigateSelectSportPage);
        }

        public string CurrentSportName
        {
            get { return currentSportName; }
            set { SetProperty(ref currentSportName, value); }
        }

        public DelegateAsyncCommand SelectSportCommand { get; }

        public override void OnAppearing()
        {
            CurrentSportName = SettingsService.CurrentSportName.ToUpperInvariant();
            sportItems = sportService.GetSportItems();

            foreach (var sportItem in sportItems)
            {
                sportItem.IsVisible = sportItem.Id == SettingsService.CurrentSportId;
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