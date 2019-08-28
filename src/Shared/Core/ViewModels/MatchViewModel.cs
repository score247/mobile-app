namespace LiveScore.Core.ViewModels
{
    using System.Linq;
    using LiveScore.Core.Converters;
    using LiveScore.Core.Events;
    using LiveScore.Core.Models.Matches;
    using LiveScore.Core.Models.Teams;
    using Prism.Events;
    using PropertyChanged;

    [AddINotifyPropertyChangedInterface]
    public class MatchViewModel
    {
        private readonly IMatchStatusConverter matchStatusConverter;
        private readonly IMatchMinuteConverter matchMinuteConverter;
        private readonly IEventAggregator eventAggregator;
        private bool isSubscribingTimer;

        public MatchViewModel(
            IMatch match,
            IDependencyResolver dependencyResolver,
            byte currentSportId)
        {
            matchStatusConverter = dependencyResolver.Resolve<IMatchStatusConverter>(currentSportId.ToString());
            matchMinuteConverter = dependencyResolver.Resolve<IMatchMinuteConverter>(currentSportId.ToString());
            eventAggregator = dependencyResolver.Resolve<IEventAggregator>();

            Match = match;
            BuildDisplayMatchStatus();
        }

        public IMatch Match { get; }

        public string DisplayMatchStatus { get; private set; }

        public void OnReceivedTeamStatistic(bool isHome, ITeamStatistic teamStatistic)
        {
            var currentTeam = Match.Teams.FirstOrDefault(t => t.IsHome == isHome);
            currentTeam.Statistic = teamStatistic;
        }

        public void OnReceivedMatchEvent(IMatchEvent matchEvent)
        {
            Match.MatchResult = matchEvent.MatchResult;
            Match.LatestTimeline = matchEvent.Timeline;

            if (matchEvent.Timeline.Type.IsPeriodStart)
            {
                Match.CurrentPeriodStartTime = matchEvent.Timeline.Time;
            }

            BuildDisplayMatchStatus();
        }

        private void BuildDisplayMatchStatus()
        {
            var matchStatus = matchStatusConverter.BuildStatus(Match);

            if (!string.IsNullOrEmpty(matchStatus))
            {
                DisplayMatchStatus = matchStatus;
                UnsubscribeMatchTimeChangeEvent();
            }
            else
            {
                BuildMatchTime();
                SubscribeMatchTimeChangeEvent();
            }
        }

        private void BuildMatchTime()
        {
            DisplayMatchStatus = matchMinuteConverter.BuildMatchMinute(Match);
        }

        private void SubscribeMatchTimeChangeEvent()
        {
            if (Match.MatchResult.EventStatus.IsLive && !isSubscribingTimer)
            {
                eventAggregator.GetEvent<OneMinuteTimerCountUpEvent>().Subscribe(BuildMatchTime);
                isSubscribingTimer = true;
            }
        }

        private void UnsubscribeMatchTimeChangeEvent()
        {
            eventAggregator.GetEvent<OneMinuteTimerCountUpEvent>().Unsubscribe(BuildMatchTime);
            isSubscribingTimer = false;
        }
    }
}