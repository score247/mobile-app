namespace Score.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Common.Models;
    using Common.ViewModels;
    using Prism.Commands;
    using Prism.Navigation;

    public class LiveViewModel : ViewModelBase
    {
        private ObservableCollection<IGrouping<string, Match>> groupMatches;
        private bool isRefreshingMatchList;

        public ObservableCollection<IGrouping<string, Match>> GroupMatches
        {
            get => groupMatches;
            set => SetProperty(ref groupMatches, value);
        }

        public bool IsRefreshingMatchList
        {
            get { return isRefreshingMatchList; }
            set { SetProperty(ref isRefreshingMatchList, value); }
        }

        public DelegateCommand RefreshCommand
            => new DelegateCommand(() =>
            {
                IsRefreshingMatchList = true;
                GetMatches();
                IsRefreshingMatchList = false;
            });

        public LiveViewModel(INavigationService navigationService)
            : base(navigationService)
        {
        }

        public override void OnAppearing()
        {
            GetMatches();
        }

        private void GetMatches()
        {
            var matches = new List<Match>();
            GroupMatches = new ObservableCollection<IGrouping<string, Match>>(matches.GroupBy(x => x.Event.League.Id));
        }
    }
}