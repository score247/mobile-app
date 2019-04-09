namespace LiveScore.League.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.ViewModels;
    using Prism.Commands;
    using Prism.Navigation;
    using Core.Factories;
    using Core.Services;
    using Core.Constants;
    using Core.Models.Leagues;
    using LiveScore.Core.Models.Matches;

    public class LeagueDetailViewModel : ViewModelBase
    {
        private readonly IMatchService matchService;
        private bool isRefreshingMatchList;
        private ILeague selectedLeagueName;
        private ObservableCollection<IGrouping<MatchHeaderItemViewModel, IMatch>> groupMatches;

        public LeagueDetailViewModel(
            INavigationService navigationService,
            IGlobalFactoryProvider globalFactory,
            ISettingsService settingsService)
                : base(navigationService, globalFactory, settingsService)
        {
            matchService = GlobalFactoryProvider.SportServiceFactoryProvider.GetInstance((SportType)SettingsService.CurrentSportId).CreateMatchService();
            GroupMatches = new ObservableCollection<IGrouping<MatchHeaderItemViewModel, IMatch>>();
        }

        public ObservableCollection<IGrouping<MatchHeaderItemViewModel, IMatch>> GroupMatches
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
                //matchService.GetMatchesByLeague(selectedLeagueName.Id, selectedLeagueName.GroupName);
                IsRefreshingMatchList = false;
            });

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters != null)
            {
                selectedLeagueName = parameters[nameof(League)] as ILeague;
                Title = selectedLeagueName?.Name ?? string.Empty;
            }

            if (GroupMatches.Count == 0)
            {
                groupMatches = await GetGroupMatchesAsync();
            }
        }

        private async Task<ObservableCollection<IGrouping<MatchHeaderItemViewModel, IMatch>>> GetGroupMatchesAsync()
        {
            IList<IMatch> matches = new List<IMatch>();

            //// TODO process grouped
            //if (!selectedLeagueName.IsGrouped)
            //{
            //    matches = await matchService.GetMatchesByLeague(selectedLeagueName.Id, selectedLeagueName.GroupName);
            //}

            //var groups = new ObservableCollection<IGrouping<MatchHeaderItemViewModel, Match>>(
            //matches.GroupBy(m => new MatchHeaderItemViewModel { Name = m.Event.LeagueRound.Number, ShortEventDate = m.Event.EventDate }));

            return null;
        }
    }
}