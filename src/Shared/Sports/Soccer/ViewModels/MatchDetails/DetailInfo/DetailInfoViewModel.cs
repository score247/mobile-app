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
using LiveScore.Soccer.Extensions;
using LiveScore.Soccer.Models.Matches;
using LiveScore.Soccer.Services;
using MethodTimer;
using Prism.Events;
using Prism.Navigation;
using Xamarin.Forms;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Soccer.Tests")]

namespace LiveScore.Soccer.ViewModels.MatchDetailInfo
{
    public class DetailInfoViewModel : TabItemViewModel
    {
        private const string SpectatorNumberFormat = "0,0";
        private readonly ISoccerMatchService matchInfoService;
        private readonly IEventAggregator eventAggregator;
        private readonly string matchId;

        public DetailInfoViewModel(
            string matchId,
            INavigationService navigationService,
            IDependencyResolver dependencyResolver,
            IEventAggregator eventAggregator,
            DataTemplate dataTemplate)
            : base(navigationService, dependencyResolver, dataTemplate, eventAggregator)
        {
            this.matchId = matchId;
            this.eventAggregator = eventAggregator;
            matchInfoService = DependencyResolver.Resolve<ISoccerMatchService>();
            RefreshCommand = new DelegateAsyncCommand(async () => await LoadDataAsync(() => LoadMatchDetail(true), false));

            eventAggregator.GetEvent<MatchEventPubSubEvent>().Subscribe(OnReceivedMatchEvent, true);
        }

        public DelegateAsyncCommand RefreshCommand { get; }

        public bool IsRefreshing { get; set; }

        public MatchInfo MatchInfo { get; private set; }

        public string DisplayEventDate { get; private set; }

        public string DisplayAttendance { get; private set; }

        public string DisplayVenue { get; private set; }

        public string DisplayReferee { get; private set; }

        public ObservableCollection<BaseItemViewModel> InfoItemViewModels { get; private set; }

        public override async Task OnNetworkReconnected()
        {
            await LoadMatchInfoData();
        }

        [Time]
        public override async void OnAppearing()
        {
            await LoadMatchInfoData();
        }

        private async Task LoadMatchInfoData()
        {
            try
            {
                // TODO: Check when need to reload data later
                await LoadDataAsync(() => LoadMatchDetail(true)).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                await LoggingService.LogExceptionAsync(ex);
            }
        }

        public override void Destroy()
        {
            eventAggregator
                .GetEvent<MatchEventPubSubEvent>()
                .Unsubscribe(OnReceivedMatchEvent);
        }

        [Time]
        private async Task LoadMatchDetail(bool isRefresh = false)
        {
            MatchInfo = await matchInfoService
                .GetMatch(matchId, CurrentLanguage, isRefresh)
                .ConfigureAwait(false);

            BuildDetailInfo(MatchInfo);

            IsRefreshing = false;
        }

        [Time]
        protected internal void OnReceivedMatchEvent(IMatchEventMessage matchEventMessage)
        {
            if (matchEventMessage.SportId != CurrentSportId
                || matchEventMessage.MatchEvent.MatchId != matchId
                || MatchInfo == null)
            {
                return;
            }

            MatchInfo.Match.UpdateResult(matchEventMessage.MatchEvent.MatchResult);

            if (MatchInfo.TimelineEvents == null)
            {
                MatchInfo.UpdateTimelineEvents(new List<TimelineEvent>());
            }

            MatchInfo.UpdateTimelineEvents(MatchInfo.TimelineEvents.Concat(
                new List<TimelineEvent> { matchEventMessage.MatchEvent.Timeline as TimelineEvent }));

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

            var soccerTimeline = matchInfo.TimelineEvents;
            soccerTimeline = soccerTimeline
                .Where(t => (t).IsDetailInfoEvent())
                .Distinct()
                .ToList();

            InfoItemViewModels = new ObservableCollection<BaseItemViewModel>(
                soccerTimeline.Select(t =>
                    BaseItemViewModel.CreateInstance(t, MatchInfo, NavigationService, DependencyResolver).BuildData()));
        }

        private void BuildFooterInfo(MatchInfo matchInfo)
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

        private static IEnumerable<TimelineEvent> FilterPenaltyEvents(MatchInfo matchInfo)
        {
            var match = matchInfo.Match;
            var timelineEvents = matchInfo.TimelineEvents.ToList();

            if (match.EventStatus.IsClosed)
            {
                timelineEvents.RemoveAll(t => t.Type == EventType.PenaltyShootout && t.IsFirstShoot);

                return timelineEvents;
            }

            if (!match.EventStatus.IsLive || !match.MatchStatus.IsInPenalties)
            {
                return timelineEvents;
            }

            var lastEvent = timelineEvents.LastOrDefault();
            timelineEvents.RemoveAll(t => t.IsFirstShoot);

            if (lastEvent?.IsFirstShoot == true)
            {
                timelineEvents.Add(lastEvent);
            }

            return timelineEvents;
        }
    }
}