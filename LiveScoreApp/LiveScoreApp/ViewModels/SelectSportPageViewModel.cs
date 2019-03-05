namespace LiveScoreApp.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Common.Settings;
    using Common.ViewModels;
    using LiveScoreApp.Models;
    using Prism.Commands;
    using Prism.Navigation;

    public class SelectSportPageViewModel : ViewModelBase
    {
        private SportItem selectedSportItem;
        private ObservableCollection<SportItem> sportItems;

        public SportItem SelectedSportItem
        {
            get => selectedSportItem;
            set => SetProperty(ref selectedSportItem, value);
        }

        public DelegateCommand SelectSportItemCommand { get; set; }

        public DelegateCommand DoneCommand { get; set; }

        public ObservableCollection<SportItem> SportItems
        {
            get => sportItems;
            set => SetProperty(ref sportItems, value);
        }

        public SelectSportPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            SelectSportItemCommand = new DelegateCommand(OnSelectSportItem);
            DoneCommand = new DelegateCommand(OnDone);
        }

        public override void OnNavigatingTo(INavigationParameters parameters)
        {
            SportItems = new ObservableCollection<SportItem>(parameters["sportItems"] as IEnumerable<SportItem>);
        }

        private async void OnSelectSportItem()
        {
            Settings.CurrentSportName = SelectedSportItem.Name;
            Settings.CurrentSportId = SelectedSportItem.Id;

            await NavigationService.GoBackAsync(useModalNavigation: true);
        }

        private async void OnDone()
        {
            await NavigationService.GoBackAsync(useModalNavigation: true);
        }
    }
}