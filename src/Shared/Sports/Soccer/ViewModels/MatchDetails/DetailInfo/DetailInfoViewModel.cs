﻿[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Soccer.Tests")]

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
    using LiveScore.Soccer.Services;
    using MethodTimer;
    using Prism.Events;
    using Prism.Navigation;
    using Xamarin.Forms;

    public class DetailInfoViewModel : TabItemViewModel, IDisposable
    {
        private const string SpectatorNumberFormat = "0,0";
        private readonly IMatchService matchService;
        private readonly IMatchInfoService matchInfoService;
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
            matchService = DependencyResolver.Resolve<IMatchService>(CurrentSportId.ToString());
            matchInfoService = DependencyResolver.Resolve<IMatchInfoService>();
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

        protected async void OnInitialized()
        {
            try
            {
                // TODO: Check when need to reload data later
                await LoadData(() => LoadMatchDetail(matchId, true)).ConfigureAwait(false);

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
            MatchInfo =
                await matchInfoService.GetMatch(matchId, CurrentLanguage, isRefresh).ConfigureAwait(false) as MatchInfo;

            BuildDetailInfo(MatchInfo);

            IsRefreshing = false;
        }

        [Time]
        protected internal void OnReceivedMatchEvent(IMatchEventMessage matchEventMessage)
        {
            if (matchEventMessage.SportId != CurrentSportId
                || matchEventMessage.MatchEvent.MatchId != matchId)
            {
                return;
            }

            MatchInfo.Match.UpdateResult(matchEventMessage.MatchEvent.MatchResult);

            if (MatchInfo.TimelineEvents == null)
            {
                MatchInfo.UpdateTimelineEvents(new List<TimelineEvent>());
            }

            MatchInfo.UpdateTimelineEvents(MatchInfo.TimelineEvents.Concat(new List<TimelineEvent> { matchEventMessage.MatchEvent.Timeline as TimelineEvent }));

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
                soccerTimeline.Select(t => new BaseItemViewModel(t, MatchInfo, NavigationService, DependencyResolver)
                    .CreateInstance()));
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