[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Soccer.Tests")]

namespace LiveScore.Soccer.ViewModels.MatchDetailInfo
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using LiveScore.Common.Extensions;
    using LiveScore.Core;
    using LiveScore.Core.Controls.TabStrip;
    using LiveScore.Core.Enumerations;
    using LiveScore.Core.Models.Matches;
    using LiveScore.Core.PubSubEvents.Matches;
    using LiveScore.Core.Services;
    using LiveScore.Soccer.Extensions;
    using LiveScore.Soccer.Models.Matches;
    using MethodTimer;
    using Prism.Events;
    using Prism.Navigation;
    using Xamarin.Forms;

    public class DetailInfoViewModel : TabItemViewModel, IDisposable
    {
        private const string SpectatorNumberFormat = "0,0";
        private readonly IMatchService matchService;
        private readonly IEventAggregator eventAggregator;
        private bool isDisposed = true;
        private readonly string matchId;

        public DetailInfoViewModel(
            string matchId,
            INavigationService navigationService,
            IDependencyResolver dependencyResolver,
            IEventAggregator eventAggregator,
            DataTemplate dataTemplate)
            : base(navigationService, dependencyResolver, dataTemplate)
        {
            this.matchId = matchId;
            this.eventAggregator = eventAggregator;
            matchService = DependencyResolver.Resolve<IMatchService>(AppSettings.CurrentSportType.Value.ToString());
            RefreshCommand = new DelegateAsyncCommand(async () => await LoadData(() => LoadMatchDetail(matchId, true), false));
            TabHeaderIcon = MatchDetailTabImage.Info;
            TabHeaderActiveIcon = MatchDetailTabImage.InfoActive;
        }

        public DelegateAsyncCommand RefreshCommand { get; }

        public bool IsRefreshing { get; set; }

        public MatchInfo MatchInfo { get; private set; }

        public string DisplayEventDate { get; private set; }

        public string DisplayAttendance { get; private set; }

        public string DisplayVenue { get; private set; }

        public string DisplayReferee { get; private set; }

        public ObservableCollection<BaseItemViewModel> InfoItemViewModels { get; private set; }

        protected override async void OnInitialized()
        {
            try
            {
                // TODO: Check when need to reload data later
                await LoadData(() => LoadMatchDetail(matchId, true));

                // TODO: need review UIThread here
                eventAggregator.GetEvent<MatchEventPubSubEvent>().Subscribe(OnReceivedMatchEvent, ThreadOption.UIThread, true);
            }
            catch (Exception ex)
            {
                await LoggingService.LogErrorAsync(ex);
            }
        }

        [Time]
        private async Task LoadMatchDetail(string matchId, bool isRefresh = false)
        {
            MatchInfo = await matchService.GetMatch(matchId, AppSettings.CurrentLanguage, isRefresh) as MatchInfo;

            BuildDetailInfo(MatchInfo);

            IsRefreshing = false;
        }

        [Time]
        protected internal void OnReceivedMatchEvent(IMatchEventMessage matchEventMessage)
        {
            if (matchEventMessage.SportId != AppSettings.CurrentSportType.Value
                || matchEventMessage.MatchEvent.MatchId != matchId)
            {
                return;
            }

            MatchInfo.Match.UpdateResult(matchEventMessage.MatchEvent.MatchResult);

            if (MatchInfo.TimelineEvents == null)
            {
                MatchInfo.UpdateTimelineEvents(new List<TimelineEvent>());
            }

            MatchInfo.UpdateTimelineEvents(MatchInfo.TimelineEvents.Concat(new[] { matchEventMessage.MatchEvent.Timeline }));

            BuildDetailInfo(MatchInfo);
        }

        private void BuildDetailInfo(MatchInfo matchInfo)
        {
            BuildInfoItems(matchInfo);
            BuildFooterInfo(matchInfo);
        }

        private void BuildInfoItems(MatchInfo matchInfo)
        {
            matchInfo.UpdateTimelineEvents(FilterPenaltyEvents(matchInfo)?.OrderByDescending(t => t.Time));

            if (matchInfo.TimelineEvents == null)
            {
                return;
            }

            var soccerTimeline = matchInfo.TimelineEvents.OfType<TimelineEvent>();
            soccerTimeline = soccerTimeline
                .Where(t => (t).IsDetailInfoEvent())
                .Distinct()
                .ToList();

            InfoItemViewModels = new ObservableCollection<BaseItemViewModel>(
                soccerTimeline.Select(t => new BaseItemViewModel(t, MatchInfo, NavigationService, DependencyResolver)
                    .CreateInstance()));
        }

        private void BuildFooterInfo(IMatchInfo matchInfo)
        {
            DisplayEventDate = matchInfo.Match.EventDate.ToFullLocalDateTime();

            if (matchInfo.Attendance > 0)
            {
                DisplayAttendance = matchInfo.Attendance.ToString(SpectatorNumberFormat);
            }

            if (matchInfo.Venue != null)
            {
                DisplayVenue = matchInfo.Venue.Name;
            }

            if (!string.IsNullOrWhiteSpace(matchInfo.Referee))
            {
                DisplayReferee = matchInfo.Referee;
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

        private static IEnumerable<ITimelineEvent> FilterPenaltyEvents(MatchInfo matchInfo)
        {
            var match = matchInfo.Match;
            var timelineEvents = matchInfo.TimelineEvents.OfType<TimelineEvent>().ToList();

            if (match.EventStatus.IsClosed)
            {
                timelineEvents.RemoveAll(t => t.Type == EventType.PenaltyShootout && t.IsFirstShoot);

                return timelineEvents;
            }

            if (match.EventStatus.IsLive && match.MatchStatus.IsInPenalties)
            {
                var lastEvent = timelineEvents.LastOrDefault();
                timelineEvents.RemoveAll(t => t.IsFirstShoot);

                if (lastEvent?.IsFirstShoot == true)
                {
                    timelineEvents.Add(lastEvent);
                }

                return timelineEvents;
            }

            return timelineEvents;
        }
    }
}