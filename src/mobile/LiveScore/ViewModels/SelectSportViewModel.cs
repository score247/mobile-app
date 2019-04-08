namespace LiveScore.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using Common.Extensions;
    using Core.ViewModels;
    using Core.Services;
    using LiveScore.Models;
    using Prism.Navigation;
    using Core.Factories;

    public class SelectSportViewModel : ViewModelBase
    {
        private SportItem selectedSportItem;
        private ObservableCollection<SportItem> sportItems;

        public SelectSportViewModel(
            INavigationService navigationService,
            IGlobalFactoryProvider globalFactory,
            ISettingsService settingsService) : base(navigationService, globalFactory, settingsService)
        {
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