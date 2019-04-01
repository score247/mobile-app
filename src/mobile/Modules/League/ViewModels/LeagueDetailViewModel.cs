namespace League.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using Common.Models;
    using Common.ViewModels;
    using League.Models;
    using League.Services;
    using Prism.Commands;
    using Prism.Navigation;

    public class LeagueDetailViewModel : ViewModelBase
    {
        private ObservableCollection<IGrouping<MatchHeaderItemViewModel, Match>> groupMatches;
        private readonly ILeagueService leagueService;
        private bool isRefreshingMatchList;
        private LeagueItem SelectedLeagueItem;

        public ObservableCollection<IGrouping<MatchHeaderItemViewModel, Match>> GroupMatches
        {
            get { return groupMatches; }
            set { SetProperty(ref groupMatches, value); }
        }

        public bool IsRefreshingMatchList
        {
            get => isRefreshingMatchList;
            set => SetProperty(ref isRefreshingMatchList, value);
        }

        public DelegateCommand RefreshCommand
            => new DelegateCommand(() =>
            {
                IsRefreshingMatchList = true;
                leagueService.GetMatchesAsync(SelectedLeagueItem.Id, SelectedLeagueItem.GroupName);
                IsRefreshingMatchList = false;
            });

        public LeagueDetailViewModel(INavigationService navigationService, ILeagueService leagueService)
            : base(navigationService)
        {
            this.leagueService = leagueService;
            GroupMatches = new ObservableCollection<IGrouping<MatchHeaderItemViewModel, Match>>();
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            SelectedLeagueItem = parameters[nameof(LeagueItem)] as LeagueItem;

            Title = SelectedLeagueItem.Name;

            Debug.Write("OnNavigatedTo LeagueDetail");

            if (GroupMatches.Count == 0)
            {
                groupMatches = await GetGroupMatchesAsync(SelectedLeagueItem);
            }
        }

        private async Task<ObservableCollection<IGrouping<MatchHeaderItemViewModel, Match>>> GetGroupMatchesAsync(LeagueItem leagueItem)
        {
            IList<Match> matches = new List<Match>();

            //TODO process grouped
            if (!SelectedLeagueItem.IsGrouped)
            {
                matches = await leagueService.GetMatchesAsync(SelectedLeagueItem.Id, SelectedLeagueItem.GroupName);
            }

            var groups = new ObservableCollection<IGrouping<MatchHeaderItemViewModel, Match>>
            (
                matches.GroupBy(m => new MatchHeaderItemViewModel { Name = m.Event.LeagueRound.Number, ShortEventDate = m.Event.EventDate })
            );

            return groups;
        }

    }
}