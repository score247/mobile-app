namespace League.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using Common.ViewModels;
    using Core.Models.LeagueInfo;
    using Core.Models.MatchInfo;
    using Core.ViewModels;
    using League.Services;
    using Prism.Commands;
    using Prism.Navigation;

    public class LeagueDetailViewModel : ViewModelBase
    {
        private readonly ILeagueService leagueService;
        private bool isRefreshingMatchList;
        private LeagueItem selectedLeagueName;
        private ObservableCollection<IGrouping<MatchHeaderItemViewModel, Match>> groupMatches;

        public LeagueDetailViewModel(INavigationService navigationService, ILeagueService leagueService)
            : base(navigationService)
        {
            this.leagueService = leagueService;
            GroupMatches = new ObservableCollection<IGrouping<MatchHeaderItemViewModel, Match>>();
        }

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
                leagueService.GetMatchesAsync(selectedLeagueName.Id, selectedLeagueName.GroupName);
                IsRefreshingMatchList = false;
            });

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters != null)
            {
                selectedLeagueName = parameters[nameof(LeagueItem)] as LeagueItem;
                Title = selectedLeagueName?.Name ?? string.Empty;
            }

            if (GroupMatches.Count == 0)
            {
                groupMatches = await GetGroupMatchesAsync();
            }
        }

        private async Task<ObservableCollection<IGrouping<MatchHeaderItemViewModel, Match>>> GetGroupMatchesAsync()
        {
            IList<Match> matches = new List<Match>();

            // TODO process grouped
            if (!selectedLeagueName.IsGrouped)
            {
                matches = await leagueService.GetMatchesAsync(selectedLeagueName.Id, selectedLeagueName.GroupName);
            }

            var groups = new ObservableCollection<IGrouping<MatchHeaderItemViewModel, Match>>(
                matches.GroupBy(m => new MatchHeaderItemViewModel { Name = m.Event.LeagueRound.Number, ShortEventDate = m.Event.EventDate }));

            return groups;
        }
    }
}