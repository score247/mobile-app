﻿namespace LiveScoreApp.ViewModels
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Common.Extensions;
    using Common.ViewModels;
    using Core.Services;
    using LiveScoreApp.Models;
    using LiveScoreApp.Services;
    using LiveScoreApp.Views;
    using Prism.Navigation;

    public class NavigationTitleViewModel : ViewModelBase
    {
        private readonly ISportService sportService;
        private readonly ISettingsService settingsService;
        private string currentSportName;
        private IEnumerable<SportItem> sportItems;

        public NavigationTitleViewModel(
            INavigationService navigationService,
            ISportService sportService,
            ISettingsService settingsService) : base(navigationService)
        {
            this.sportService = sportService;
            this.settingsService = settingsService;
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
            CurrentSportName = settingsService.CurrentSportName.ToUpperInvariant();
            sportItems = sportService.GetSportItems();

            foreach (var sportItem in sportItems)
            {
                sportItem.IsVisible = sportItem.Id == settingsService.CurrentSportId;
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