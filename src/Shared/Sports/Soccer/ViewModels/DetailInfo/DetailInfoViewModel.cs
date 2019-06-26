using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Soccer.Tests")]

namespace LiveScore.Soccer.ViewModels.MatchDetailInfo
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Core.ViewModels;
    using LiveScore.Common.Extensions;
    using LiveScore.Core;
    using LiveScore.Core.Models.Matches;
    using LiveScore.Core.Services;
    using LiveScore.Soccer.Models.Matches;
    using Microsoft.AspNetCore.SignalR.Client;
    using Prism.Navigation;

    public class DetailInfoViewModel : ViewModelBase, IDisposable
    {
        private const string SpectatorNumberFormat = "0,0";
        private static readonly TimeSpan HubKeepAliveInterval = TimeSpan.FromSeconds(30);
        private readonly HubConnection matchHubConnection;
        private readonly IMatchService matchService;
        private CancellationTokenSource cancellationTokenSource;
        private bool disposedValue;
        private readonly string matchId;

        public DetailInfoViewModel(
            string matchId,
            INavigationService navigationService,
            IDependencyResolver dependencyResolver,
            HubConnection matchHubConnection)
            : base(navigationService, dependencyResolver)
        {
            this.matchId = matchId;
            this.matchHubConnection = matchHubConnection;
            matchService = DependencyResolver.Resolve<IMatchService>(SettingsService.CurrentSportType.Value);
            RefreshCommand = new DelegateAsyncCommand(async () => await LoadMatchDetail(Match.Id, false, true));
        }

        public DelegateAsyncCommand RefreshCommand { get; }

        public bool IsRefreshing { get; set; }

        public bool IsLoading { get; private set; }

        public bool IsNotLoading { get; private set; }

        public IMatch Match { get; private set; }

        public string DisplayEventDate { get; private set; }

        public string DisplayAttendance { get; private set; }

        public string DisplayVenue { get; private set; }

        public ObservableCollection<BaseItemViewModel> InfoItemViewModels { get; private set; }

        protected override void Clean()
        {
            base.Clean();

            cancellationTokenSource?.Dispose();
        }

        protected override async void Initialize()
        {
            try
            {
                await LoadMatchDetail(matchId);
                cancellationTokenSource = new CancellationTokenSource();

                await StartListeningMatchHubEvent();
            }
            catch (Exception ex)
            {
                await LoggingService.LogErrorAsync(ex);
            }
        }

        private async Task LoadMatchDetail(string matchId, bool showLoadingIndicator = true, bool isRefresh = false)
        {
            IsLoading = showLoadingIndicator;

            Match = await matchService.GetMatch(SettingsService.UserSettings, matchId, isRefresh);

            BuildDetailInfo(Match);

            IsLoading = false;
            IsNotLoading = true;
            IsRefreshing = false;
        }

        private async Task StartListeningMatchHubEvent()
        {
            matchHubConnection.On<string, Dictionary<string, MatchPushEvent>>("PushMatchEvent", OnReceivingMatchEvent);

            await matchHubConnection.StartWithKeepAlive(HubKeepAliveInterval, cancellationTokenSource.Token);
        }

        protected internal void OnReceivingMatchEvent(string sportId, Dictionary<string, MatchPushEvent> payload)
        {
            if (sportId != SettingsService.CurrentSportType.Value || Match?.Id == null || !payload.ContainsKey(Match.Id))
            {
                return;
            }

            var matchPayload = payload[Match.Id];
            Match.MatchResult = matchPayload.MatchResult;

            if (Match.TimeLines == null)
            {
                Match.TimeLines = new List<Timeline>();
            }

            Match.TimeLines = Match.TimeLines.Concat(matchPayload.TimeLines).Distinct(new TimelineComparer());

            BuildDetailInfo(Match);
        }

        private void BuildDetailInfo(IMatch match)
        {
            BuildInfoItems(match);
            BuildFooterInfo(match);
        }

        private void BuildInfoItems(IMatch match)
        {
            match.TimeLines = BaseItemViewModel.FilterPenaltyEvents(match?.TimeLines, match?.MatchResult);
            var timelines = match.TimeLines?
              .Where(t => BaseItemViewModel.ValidateEvent(t, match.MatchResult))
              .OrderBy(t => t.Time).ToList() ?? new List<ITimeline>();

            InfoItemViewModels = new ObservableCollection<BaseItemViewModel>(timelines.Select(t =>
                   new BaseItemViewModel(t, match.MatchResult, NavigationService, DependencyResolver)
                   .CreateInstance()));
        }

        private void BuildFooterInfo(IMatch match)
        {
            DisplayEventDate = match.EventDate.ToFullDateTime();

            if (match.Attendance > 0)
            {
                DisplayAttendance = match.Attendance.ToString(SpectatorNumberFormat);
            }

            if (match.Venue != null)
            {
                DisplayVenue = match.Venue.Name;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                // Not use dispose method because of keeping long using object, handling object is implemented in Clean()
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}