namespace LiveScoreApp.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Common.Settings;
    using Common.ViewModels;
    using LiveScoreApp.Models;
    using Prism.Commands;
    using Prism.Navigation;

    public class SelectSportViewModel : ViewModelBase
    {
        private SportItem selectedSportItem;
        private ObservableCollection<SportItem> sportItems;

        public SelectSportViewModel(INavigationService navigationService)
          : base(navigationService)
        {
            SelectSportItemCommand = new DelegateCommand(OnSelectSportItem);
            DoneCommand = new DelegateCommand(OnDone);
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


        public DelegateCommand SelectSportItemCommand { get; set; }

        public DelegateCommand DoneCommand { get; set; }


        public override void OnNavigatingTo(INavigationParameters parameters)
        {
            SportItems = new ObservableCollection<SportItem>(parameters["sportItems"] as IEnumerable<SportItem>);
        }

        private async void OnSelectSportItem()
        {
            var parameters = new NavigationParameters
            {
                { "changeSport", Settings.CurrentSportId != SelectedSportItem.Id && SelectedSportItem.Id != 0 }
            };

            if (SelectedSportItem.Id != 0)
            {
                Settings.CurrentSportName = SelectedSportItem.Name;
                Settings.CurrentSportId = SelectedSportItem.Id;
            }

            await NavigationService.GoBackAsync(useModalNavigation: true, parameters: parameters);
        }

        private async void OnDone()
        {
            await NavigationService.GoBackAsync(useModalNavigation: true);
        }
    }
}