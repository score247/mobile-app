namespace LiveScoreApp.ViewModels
{
    using LiveScoreApp.Models;
    using LiveScoreApp.Services;
    using LiveScoreApp.Settings;
    using Prism.Commands;
    using Prism.Navigation;
    using System.Collections.Generic;
    using System.Linq;

    public class NavigationTitleViewViewModel : ViewModelBase
    {
        private string currentSportName;
        private IEnumerable<SportItem> sportItems;
        private readonly ISportService sportService;

        public string CurrentSportName
        {
            get { return currentSportName; }
            set { SetProperty(ref currentSportName, value); }
        }

        public DelegateCommand SelectSportCommand { get; set; }

        public NavigationTitleViewViewModel(INavigationService navigationService, ISportService sportService)
            : base(navigationService)
        {
            this.sportService = sportService;
            GetAndSetCurrentSport();
            SelectSportCommand = new DelegateCommand(NavigateSelectSportPage);
        }

        public override void OnAppearing()
        {
            CurrentSportName = Settings.CurrentSportName;

            foreach (var sportItem in sportItems)
            {
                sportItem.IsVisible = sportItem.Id == Settings.CurrentSportId;
            }
        }

        private void GetAndSetCurrentSport()
        {
            sportItems = sportService.GetSportItems();
            var currentSport = sportItems.FirstOrDefault(s => s.Id == 1);
            currentSport.IsVisible = true;
            Settings.CurrentSportName = currentSport?.Name ?? "Soccer";
            Settings.CurrentSportId = currentSport?.Id ?? 1;
        }

        private async void NavigateSelectSportPage()
        {
            var parameters = new NavigationParameters();
            parameters.Add(nameof(sportItems), sportItems);

            await NavigationService.NavigateAsync("NavigationPage/SelectSportPage", parameters, useModalNavigation: true);
        }
    }
}