namespace LiveScore.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using Common.Extensions;
    using Core.Factories;
    using Core.Services;
    using Core.ViewModels;
    using LiveScore.Models;
    using Prism.Navigation;

    public class SelectSportViewModel : ViewModelBase
    {
        public SelectSportViewModel(
            INavigationService navigationService,
            IGlobalFactoryProvider globalFactory,
            ISettingsService settingsService) : base(navigationService, globalFactory, settingsService)
        {
            SelectSportItemCommand = new DelegateAsyncCommand(OnSelectSportItem);
            DoneCommand = new DelegateAsyncCommand(OnDone);
        }

        public SportItem SelectedSportItem { get; set; }

        public ObservableCollection<SportItem> SportItems { get; set; }

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
                { "changeSport", SettingsService.CurrentSportId != SelectedSportItem.Id && SelectedSportItem.Id != 0 }
            };

            if (SelectedSportItem.Id != 0)
            {
                SettingsService.CurrentSportName = SelectedSportItem.Name;
                SettingsService.CurrentSportId = SelectedSportItem.Id;
            }

            await NavigationService.GoBackAsync(useModalNavigation: true, parameters: parameters);
        }

        private async Task OnDone()
        {
            await NavigationService.GoBackAsync(useModalNavigation: true);
        }
    }
}