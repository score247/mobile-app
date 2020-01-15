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
using MethodTimer;
using Prism.Events;
using Prism.Navigation;
using Xamarin.Forms;

[assembly: InternalsVisibleTo("Soccer.Tests")]

namespace LiveScore.Soccer.ViewModels.Matches.MatchDetails.HeadToHead
{
    public class H2HViewModel : TabItemViewModel, IDisposable
    {
        private const string HomeIdentifier = "home";

        private readonly IMatch match;
        private readonly ITeamService teamService;
        private readonly IMatchDisplayStatusBuilder matchStatusBuilder;
        private readonly Func<string, string> buildFlagUrlFunc;
        private bool disposed;

        public H2HViewModel(
            IMatch match,
            INavigationService navigationService,
            IDependencyResolver dependencyResolver,
            IEventAggregator eventAggregator,
            DataTemplate dataTemplate)
            : base(navigationService, dependencyResolver, dataTemplate, eventAggregator, AppResources.H2H)
        {
            this.match = match;
            matchStatusBuilder = DependencyResolver.Resolve<IMatchDisplayStatusBuilder>(CurrentSportId.ToString());
            buildFlagUrlFunc = DependencyResolver.Resolve<Func<string, string>>(FuncNameConstants.BuildFlagUrlFuncName);
            teamService = DependencyResolver.Resolve<ITeamService>(CurrentSportId.ToString());

            OnTeamResultTappedCommand = new DelegateAsyncCommand<string>(OnTeamResultTapped);
            OnHeadToHeadTappedCommand = new DelegateAsyncCommand(OnHeadToHeadTapped);
            RefreshCommand = new DelegateAsyncCommand(RefreshAsync);
            Initialize();
        }

        public bool IsRefreshing { get; set; }

        public bool VisibleHeadToHead { get; private set; }

        public bool VisibleStats { get; private set; }

        public string HomeTeamName { get; private set; }

        public string AwayTeamName { get; private set; }

        public string SelectedTeamIdentifier { get; private set; }

        public H2HStatisticViewModel Stats { get; private set; }

        public DelegateAsyncCommand<string> OnTeamResultTappedCommand { get; }

        public DelegateAsyncCommand OnHeadToHeadTappedCommand { get; }

        public DelegateAsyncCommand RefreshCommand { get; }

        public ObservableCollection<H2HMatchGroupViewModel> GroupedMatches { get; set; }

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

            if (GroupedMatches?.Any() == false)
            {
                await RefreshAsync();
            }
        }

        public override Task OnNetworkReconnectedAsync() => RefreshAsync();

        public override void Destroy()
        {
            base.Destroy();

            Dispose();
        }

        [Time]
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

        internal Task OnTeamResultTapped(string teamIdentifier)
        {
            IsFirstLoad = true;

            return LoadDataAsync(() => LoadTeamResultAsync(teamIdentifier));
        }

        internal Task OnHeadToHeadTapped()
        {
            IsFirstLoad = true;

            return LoadDataAsync(LoadHeadToHeadAsync);
        }

        [Time]
        internal Task LoadTeamResultAsync(string teamIdentifier)
        {
            SelectedTeamIdentifier = teamIdentifier;
            VisibleHeadToHead = false;
            VisibleStats = false;

            return GetAndBindingMatchesAsync(teamIdentifier);
        }

        [Time]
        internal async Task LoadHeadToHeadAsync()
        {
            VisibleHeadToHead = true;
            SelectedTeamIdentifier = string.Empty;

            var headToHeads = await GetAndBindingMatchesAsync();

            Stats = GenerateStatsViewModel(headToHeads.Where(m => m.EventStatus.IsClosed).ToList());
            VisibleStats = Stats?.Total > 0;
        }

        internal async Task<IEnumerable<IMatch>> GetAndBindingMatchesAsync(string teamIdentifier = null)
        {
            // To hide No Data label
            HasData = true;

            if (!IsRefreshing)
            {
                Device.BeginInvokeOnMainThread(() => GroupedMatches?.Clear());
            }

            var matchList = (await GetMatchesAsync(teamIdentifier))?.ToList();

            if (matchList?.Any() != true)
            {
                HasData = false;

                return Enumerable.Empty<IMatch>();
            }

            var matches = BuildMatchGroups(matchList);

            Device.BeginInvokeOnMainThread(() => GroupedMatches = new ObservableCollection<H2HMatchGroupViewModel>(matches));

            HasData = true;

            return matchList;
        }

        internal async Task<IEnumerable<IMatch>> GetMatchesAsync(string teamIdentifier = null)
        {
            try
            {
                var (teamId, opponentTeamId) = GetTeamAndOpponent(teamIdentifier);

                return VisibleHeadToHead
                                ? (await teamService.GetHeadToHeadsAsync(
                                        match.HomeTeamId,
                                        match.AwayTeamId,
                                        CurrentLanguage.DisplayName)
                                    .ConfigureAwait(false))
                                    ?.Except(new List<IMatch> { match }).ToList()
                                : await teamService.GetTeamResultsAsync(
                                        teamId,
                                        opponentTeamId,
                                        CurrentLanguage.DisplayName)
                                    .ConfigureAwait(false);
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
                    .OrderByDescending(m => m.EventDate)
                    .Select(m => new H2HMatchViewModel(VisibleHeadToHead, selectedTeamId, m, matchStatusBuilder))
                    .GroupBy(item => new H2HMatchGrouping(item.Match));

            return matchGroups.Select(group => new H2HMatchGroupViewModel(group.ToList(), buildFlagUrlFunc));
        }

        private H2HStatisticViewModel GenerateStatsViewModel(ICollection<IMatch> closedMatches)
        => closedMatches?.Any() == true
                ? new H2HStatisticViewModel(
                    closedMatches.Count(x => x.WinnerId == match.HomeTeamId),
                    closedMatches.Count(x => x.WinnerId == match.AwayTeamId),
                    closedMatches.Count)
                : null;

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }

            if (disposing)
            {
                GroupedMatches = null;
            }

            disposed = true;
        }
    }
}