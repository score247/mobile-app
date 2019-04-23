﻿namespace LiveScore.Core.ViewModels
{
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using Common.Extensions;
    using LiveScore.Core;
    using LiveScore.Core.Models;
    using LiveScore.Core.Services;
    using Prism.Navigation;

    public class SelectSportViewModel : ViewModelBase
    {
        private readonly ISportService sportService;

        public SelectSportViewModel(
            INavigationService navigationService,
            IDepdendencyResolver dependencyResolver,
            ISportService sportService)
                : base(navigationService, dependencyResolver)
        {
            this.sportService = sportService;
            SelectSportItemCommand = new DelegateAsyncCommand(OnSelectSportItem);
            DoneCommand = new DelegateAsyncCommand(OnDone);
        }

        public SportItem SelectedSportItem { get; set; }

        public ObservableCollection<SportItem> SportItems { get; private set; }

        public DelegateAsyncCommand SelectSportItemCommand { get; }

        public DelegateAsyncCommand DoneCommand { get; }

        public override void OnNavigatingTo(INavigationParameters parameters)
        {
            var sportItems = sportService.GetSportItems();

            foreach (var sportItem in sportItems)
            {
                sportItem.IsVisible = sportItem.Type.Value == SettingsService.CurrentSportType.Value;
            }

            SportItems = new ObservableCollection<SportItem>(sportItems);
        }

        private async Task OnSelectSportItem()
        {
            if (SelectedSportItem.Type != null)
            {
                var isSportChanged = SettingsService.CurrentSportType.Value != SelectedSportItem.Type.Value;
                SettingsService.CurrentSportType = SelectedSportItem.Type;

                if (isSportChanged)
                {
                    await NavigateToHome();
                }
            }
        }

        private async Task OnDone()
        {
            await NavigationService.GoBackAsync(useModalNavigation: true);
        }
    }
}