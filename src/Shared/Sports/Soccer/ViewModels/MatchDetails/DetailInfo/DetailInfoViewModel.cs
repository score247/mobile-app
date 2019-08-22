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
    using LiveScore.Common.Extensions;
    using LiveScore.Core;
    using LiveScore.Core.Controls.TabStrip;
    using LiveScore.Core.Enumerations;
    using LiveScore.Core.Models.Matches;
    using LiveScore.Core.Services;
    using LiveScore.Soccer.Extensions;
    using LiveScore.Soccer.Models.Matches;
    using MethodTimer;
    using Microsoft.AspNetCore.SignalR.Client;
    using Newtonsoft.Json;
    using Prism.Navigation;
    using Xamarin.Forms;

    public class DetailInfoViewModel : TabItemViewModelBase, IDisposable
    {
        private const string SpectatorNumberFormat = "0,0";
        private static readonly TimeSpan HubKeepAliveInterval = TimeSpan.FromSeconds(30);
        private readonly HubConnection matchHubConnection;
        private readonly IMatchService matchService;
        private CancellationTokenSource cancellationTokenSource;
        private bool isDisposed = true;
        private readonly string matchId;

        public DetailInfoViewModel(
            string matchId,
            INavigationService navigationService,
            IDependencyResolver dependencyResolver,
            HubConnection matchHubConnection,
            DataTemplate dataTemplate)
            : base(navigationService, dependencyResolver, dataTemplate)
        {
            this.matchId = matchId;
            this.matchHubConnection = matchHubConnection;
            matchService = DependencyResolver.Resolve<IMatchService>(SettingsService.CurrentSportType.Value.ToString());
            RefreshCommand = new DelegateAsyncCommand(async () => await LoadData(() => LoadMatchDetail(Match.Id, true), false));

            TabHeaderIcon = TabDetailImages.Info;
            TabHeaderActiveIcon = TabDetailImages.InfoActive;
        }

        public DelegateAsyncCommand RefreshCommand { get; }

        public bool IsRefreshing { get; set; }

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
                cancellationTokenSource = new CancellationTokenSource();

                // TODO: Check when need to reload data later
                await LoadData(() => LoadMatchDetail(matchId, true));

                await StartListeningMatchHubEvent();
            }
            catch (Exception ex)
            {
                await LoggingService.LogErrorAsync(ex);
            }
        }

        [Time]
        private async Task LoadMatchDetail(string matchId, bool isRefresh = false)
        {
            Match = await matchService.GetMatch(matchId, SettingsService.Language, isRefresh);

            BuildDetailInfo(Match);

            IsRefreshing = false;
        }

        private async Task StartListeningMatchHubEvent()
        {
            matchHubConnection.On<byte, string>("MatchEvent", OnReceivedMatchEvent);

            await matchHubConnection.StartWithKeepAlive(HubKeepAliveInterval, LoggingService, cancellationTokenSource.Token);
        }

        [Time]
        protected internal void OnReceivedMatchEvent(byte sportId, string payload)
        {
            var matchEvent = JsonConvert.DeserializeObject<MatchEvent>(payload);

            if (sportId != SettingsService.CurrentSportType.Value || Match?.Id == null || matchEvent.MatchId != Match.Id)
            {
                return;
            }

            Match.MatchResult = matchEvent.MatchResult;

            if (Match.TimeLines == null)
            {
                Match.TimeLines = new List<TimelineEvent>();
            }

            Match.TimeLines = Match.TimeLines.Concat(new[] { matchEvent.Timeline });

            BuildDetailInfo(Match);
        }

        private void BuildDetailInfo(IMatch match)
        {
            BuildInfoItems(match);
            BuildFooterInfo(match);
        }

        private void BuildInfoItems(IMatch match)
        {
            match.TimeLines = FilterPenaltyEvents(match?.TimeLines, match?.MatchResult)?.OrderByDescending(t => t.Time);

            if (match.TimeLines != null)
            {
                var timelines = match.TimeLines
                    .Where(t => t.IsDetailInfoEvent())
                    .Distinct(new TimelineComparer()).ToList() ?? new List<ITimelineEvent>(); // TODO: Replace TimelineComparer

                InfoItemViewModels = new ObservableCollection<BaseItemViewModel>(timelines.Select(t =>
                       new BaseItemViewModel(t, match.MatchResult, NavigationService, DependencyResolver)
                       .CreateInstance()));
            }
        }

        private void BuildFooterInfo(IMatch match)
        {
            DisplayEventDate = match.EventDate.ToFullLocalDateTime();

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
            if (!isDisposed)
            {
                // Not use dispose method because of keeping long using object, handling object is implemented in Clean()
                isDisposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private static IEnumerable<ITimelineEvent> FilterPenaltyEvents(IEnumerable<ITimelineEvent> timelines, IMatchResult matchResult)
        {
            if (matchResult == null)
            {
                return timelines;
            }

            if (matchResult.EventStatus.IsClosed)
            {
                var timelineEvents = timelines.ToList();
                timelineEvents.RemoveAll(t => t.Type == EventType.PenaltyShootout && t.IsFirstShoot);

                return timelineEvents;
            }

            if (matchResult.EventStatus.IsLive && matchResult.MatchStatus.IsInPenalties)
            {
                var lastEvent = timelines.LastOrDefault();
                var timelineEvents = timelines.ToList();
                timelineEvents.RemoveAll(t => t.IsFirstShoot);

                if (lastEvent?.IsFirstShoot == true)
                {
                    timelineEvents.Add(lastEvent);
                }

                return timelineEvents;
            }

            return timelines;
        }
    }
}