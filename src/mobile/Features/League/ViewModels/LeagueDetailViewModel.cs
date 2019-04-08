namespace LiveScore.League.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.ViewModels;
    using Core.Models.LeagueInfo;
    using Core.Models.MatchInfo;
    using Prism.Commands;
    using Prism.Navigation;
    using Core.Factories;
    using Core.Services;
    using Core.Constants;

    public class LeagueDetailViewModel : ViewModelBase
    {
        private readonly IMatchService matchService;
        private bool isRefreshingMatchList;
        private LeagueItem selectedLeagueName;
        private ObservableCollection<IGrouping<MatchHeaderItemViewModel, Match>> groupMatches;

        public LeagueDetailViewModel(
            INavigationService navigationService,
            IGlobalFactoryProvider globalFactory,
            ISettingsService settingsService)
                : base(navigationService, globalFactory, settingsService)
        {
            matchService = GlobalFactory.SportServiceFactoryProvider.GetInstance((SportType)SettingsService.CurrentSportId).CreateMatchService();
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
                matchService.GetMatchesByLeague(selectedLeagueName.Id, selectedLeagueName.GroupName);
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
                matches = await matchService.GetMatchesByLeague(selectedLeagueName.Id, selectedLeagueName.GroupName);
            }

            var groups = new ObservableCollection<IGrouping<MatchHeaderItemViewModel, Match>>(
                matches.GroupBy(m => new MatchHeaderItemViewModel { Name = m.Event.LeagueRound.Number, ShortEventDate = m.Event.EventDate }));

            return groups;
        }
    }
}