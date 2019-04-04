﻿namespace LiveScoreApp.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using Common.Extensions;
    using Common.ViewModels;
    using Core.Services;
    using LiveScoreApp.Models;
    using Prism.Navigation;

    public class SelectSportViewModel : ViewModelBase
    {
        private readonly ISettingsService settingsService;
        private SportItem selectedSportItem;
        private ObservableCollection<SportItem> sportItems;

        public SelectSportViewModel(INavigationService navigationService, ISettingsService settingsService)
          : base(navigationService)
        {
            this.settingsService = settingsService;
            SelectSportItemCommand = new DelegateAsyncCommand(OnSelectSportItem);
            DoneCommand = new DelegateAsyncCommand(OnDone);
        }

        public SportItem SelectedSportItem
        {
            get => selectedSportItem;
            set => SetProperty(ref selectedSportItem, value);
        }

        public ObservableCollection<SportItem> SportItems
        {
            get => sportItems;
            set => SetProperty(ref sportItems, value);
        }

        public DelegateAsyncCommand SelectSportItemCommand { get; set; }

        public DelegateAsyncCommand DoneCommand { get; set; }

        public override void OnNavigatingTo(INavigationParameters parameters)
        {
            SportItems = new ObservableCollection<SportItem>(parameters["sportItems"] as IEnumerable<SportItem>);
        }

        private async Task OnSelectSportItem()
        {
            var parameters = new NavigationParameters
            {
                { "changeSport", settingsService.CurrentSportId != SelectedSportItem.Id && SelectedSportItem.Id != 0 }
            };

            if (SelectedSportItem.Id != 0)
            {
                settingsService.CurrentSportName = SelectedSportItem.Name;
                settingsService.CurrentSportId = SelectedSportItem.Id;
            }

            await NavigationService.GoBackAsync(useModalNavigation: true, parameters: parameters);
        }

        private async Task OnDone()
        {
            await NavigationService.GoBackAsync(useModalNavigation: true);
        }
    }
}