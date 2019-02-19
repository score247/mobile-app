namespace LiveScoreApp.ViewModels
{
    using LiveScoreApp.Models;
    using LiveScoreApp.Services;
    using Prism.Navigation;
    using System.Collections.ObjectModel;

    public class SelectSportPageViewModel : ViewModelBase
    {
        private SportItem selectedSportItem;

        public SportItem SelectedSportItem
        {
            get => selectedSportItem;
            set => SetProperty(ref selectedSportItem, value);
        }

        public ObservableCollection<SportItem> SportItems { get; set; }

        public SelectSportPageViewModel(INavigationService navigationService, ISportService sportService)
            : base(navigationService)
        {
            SportItems = new ObservableCollection<SportItem>(sportService.GetSportItems());
        }
    }
}