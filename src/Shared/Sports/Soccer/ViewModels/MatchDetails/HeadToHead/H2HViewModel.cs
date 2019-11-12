using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using LiveScore.Common;
using LiveScore.Common.Extensions;
using LiveScore.Common.LangResources;
using LiveScore.Core;
using LiveScore.Core.Controls.TabStrip;
using LiveScore.Core.Converters;
using LiveScore.Core.Models.Matches;
using LiveScore.Core.Services;
using Prism.Events;
using Prism.Navigation;
using Xamarin.Forms;

[assembly: InternalsVisibleTo("Soccer.Tests")]

namespace LiveScore.Soccer.ViewModels.MatchDetails.HeadToHead
{
    public class H2HViewModel : TabItemViewModel
    {
        private const string HomeIdentifier = "home";
        private const string AwayIdentifier = "away";

        private readonly IMatch match;
        private readonly ITeamService teamService;
        private readonly IMatchDisplayStatusBuilder matchStatusBuilder;
        private readonly Func<string, string> buildFlagUrlFunc;

        public H2HViewModel(
            IMatch match,
            INavigationService navigationService,
            IDependencyResolver dependencyResolver,
            IEventAggregator eventAggregator,
            DataTemplate dataTemplate)
            : base(navigationService, dependencyResolver, dataTemplate, eventAggregator, AppResources.H2H)
        {
            this.match = match;
            Initialize();

            matchStatusBuilder = DependencyResolver.Resolve<IMatchDisplayStatusBuilder>(CurrentSportId.ToString());
            buildFlagUrlFunc = DependencyResolver.Resolve<Func<string, string>>(FuncNameConstants.BuildFlagUrlFuncName);

            teamService = DependencyResolver.Resolve<ITeamService>(CurrentSportId.ToString());

            OnTeamResultTapped = new DelegateAsyncCommand<string>((teamIdentifier) => LoadDataAsync(() => LoadTeamResultAsync(teamIdentifier)));
            OnHeadToHeadTapped = new DelegateAsyncCommand(() => LoadDataAsync(LoadHeadToHeadAsync));
            RefreshCommand = new DelegateAsyncCommand(RefreshAsync);
        }

        public bool IsRefreshing { get; set; }

        public bool VisibleTeamResults => !VisibleHeadToHead;

        public bool VisibleHomeResults => VisibleTeamResults && SelectedTeamIdentifier == HomeIdentifier;

        public bool VisibleAwayResults => VisibleTeamResults && SelectedTeamIdentifier == AwayIdentifier;

        public bool VisibleHeadToHead { get; private set; }

        public bool VisibleStats { get; private set; }

        public string HomeTeamName { get; private set; }

        public string AwayTeamName { get; private set; }

        public string SelectedTeamIdentifier { get; private set; }

        public H2HStatisticViewModel Stats { get; private set; }

        public DelegateAsyncCommand<string> OnTeamResultTapped { get; }

        public DelegateAsyncCommand OnHeadToHeadTapped { get; }

        public DelegateAsyncCommand RefreshCommand { get; }

        public ObservableCollection<H2HMatchGroupViewModel> HeadToHeadMatches { get; set; }

        public ObservableCollection<H2HMatchGroupViewModel> TeamMatches { get; set; }

        private void Initialize()
        {
            HomeTeamName = match.HomeTeamName;
            AwayTeamName = match.AwayTeamName;

            VisibleHeadToHead = true;
            HasData = true;
        }

        public override async void OnAppearing()
        {
            base.OnAppearing();

            if (IsFirstLoad)
            {
                await LoadDataAsync(LoadHeadToHeadAsync);
            }
        }

        public override async void OnResumeWhenNetworkOK()
        {
            base.OnResumeWhenNetworkOK();

            await RefreshAsync();
        }

        public override Task OnNetworkReconnectedAsync() => RefreshAsync();

        internal async Task RefreshAsync()
        {
            IsRefreshing = true;

            await LoadDataAsync(async () =>
            {
                if (VisibleHeadToHead)
                {
                    await LoadHeadToHeadAsync();
                }
                else
                {
                    await LoadTeamResultAsync(SelectedTeamIdentifier);
                }
            });

            IsRefreshing = false;
        }

        internal async Task LoadTeamResultAsync(string teamIdentifier)
        {
            SelectedTeamIdentifier = teamIdentifier;
            VisibleHeadToHead = false;

            Device.BeginInvokeOnMainThread(() => TeamMatches?.Clear());

            var teamResults = await GetMatchesAsync(teamIdentifier);

            if (teamResults == null || !teamResults.Any())
            {
                HasData = false;
                return;
            }

            TeamMatches = new ObservableCollection<H2HMatchGroupViewModel>(BuildMatchGroups(teamResults));

            HasData = true;
        }

        internal async Task LoadHeadToHeadAsync()
        {
            VisibleHeadToHead = true;

            Device.BeginInvokeOnMainThread(() => HeadToHeadMatches?.Clear());

            var headToHeads = await GetMatchesAsync();

            if (headToHeads == null || !headToHeads.Any())
            {
                HasData = false;
                return;
            }

            Stats = GenerateStatsViewModel(headToHeads.Where(match => match.EventStatus.IsClosed));
            VisibleStats = Stats?.Total > 0;

            HeadToHeadMatches = new ObservableCollection<H2HMatchGroupViewModel>(BuildMatchGroups(headToHeads));

            HasData = true;
        }

        internal async Task<IEnumerable<IMatch>> GetMatchesAsync(string teamIdentifier = null)
        {
            try
            {
                var teamPair = GetTeamAndOpponent(teamIdentifier);

                return VisibleHeadToHead
                                ? (await teamService.GetHeadToHeadsAsync(
                                        match.HomeTeamId,
                                        match.AwayTeamId,
                                        CurrentLanguage.DisplayName)
                                    .ConfigureAwait(false))
                                    ?.Except(new List<IMatch> { match }).ToList()
                                : await teamService.GetTeamResultsAsync(teamPair.Key, teamPair.Value, CurrentLanguage.DisplayName);
            }
            catch (Exception ex)
            {
                await LoggingService.LogExceptionAsync(ex);

                HasData = false;

                return null;
            }
        }

        private KeyValuePair<string, string> GetTeamAndOpponent(string teamIdentifier)
            => teamIdentifier == HomeIdentifier
                    ? new KeyValuePair<string, string>(match.HomeTeamId, match.AwayTeamId)
                    : new KeyValuePair<string, string>(match.AwayTeamId, match.HomeTeamId);

        private IEnumerable<H2HMatchGroupViewModel> BuildMatchGroups(IEnumerable<IMatch> headToHeads)
        {
            var selectedTeamId = SelectedTeamIdentifier == HomeIdentifier ? match.HomeTeamId : match.AwayTeamId;

            var matchGroups
                = headToHeads
                    .OrderByDescending(match => match.EventDate)
                    .Select(match => new H2HMatchViewModel(VisibleHeadToHead, selectedTeamId, match, matchStatusBuilder))
                    .GroupBy(item => new H2HMatchGrouping(item.Match));

            return matchGroups.Select(group => new H2HMatchGroupViewModel(group.ToList(), buildFlagUrlFunc));
        }

        private H2HStatisticViewModel GenerateStatsViewModel(IEnumerable<IMatch> closedMatches)
        => closedMatches?.Any() == true
                ? new H2HStatisticViewModel(
                    closedMatches.Count(x => x.WinnerId == match.HomeTeamId),
                    closedMatches.Count(x => x.WinnerId == match.AwayTeamId),
                    closedMatches.Count())
                : null;
    }
}