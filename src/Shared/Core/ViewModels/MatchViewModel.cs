using LiveScore.Core.Converters;
using LiveScore.Core.Events;
using LiveScore.Core.Models.Matches;
using LiveScore.Core.Models.Teams;
using MvvmHelpers;
using Prism.Events;
using PropertyChanged;

namespace LiveScore.Core.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class MatchViewModel : BaseViewModel
    {
        private bool isSubscribingTimer;
        private readonly IMatchDisplayStatusBuilder matchDisplayStatusBuilder;
        private readonly IMatchMinuteBuilder matchMinuteConverter;
        private readonly IEventAggregator eventAggregator;

        public MatchViewModel(
            IMatch match,
            IMatchDisplayStatusBuilder matchDisplayStatusBuilder,
            IMatchMinuteBuilder matchMinuteBuilder,
            IEventAggregator eventAggregator,
            bool isBusy = false)
        {
            this.matchDisplayStatusBuilder = matchDisplayStatusBuilder;
            this.matchMinuteConverter = matchMinuteBuilder;
            this.eventAggregator = eventAggregator;
            IsBusy = isBusy;

            BuildMatch(match);
        }

        public IMatch Match { get; private set; }

        public string DisplayMatchStatus { get; private set; }

        public void BuildMatch(IMatch match)
        {
            Match = match;
            BuildDisplayMatchStatus();
        }

        public void UpdateMatch(IMatch match)
        {
            Match.UpdateMatch(match);
            BuildDisplayMatchStatus();
        }

        public void OnReceivedTeamStatistic(bool isHome, ITeamStatistic teamStatistic)
        {
            Match.UpdateTeamStatistic(teamStatistic, isHome);
        }

        public void OnReceivedMatchEvent(IMatchEvent matchEvent)
        {
            Match.UpdateLastTimeline(matchEvent.Timeline);
            Match.UpdateResult(matchEvent.MatchResult);

            if (matchEvent.Timeline.Type.IsPeriodStart)
            {
                Match.CurrentPeriodStartTime = matchEvent.Timeline.Time;
            }

            BuildDisplayMatchStatus();
        }

        private void BuildDisplayMatchStatus()
        {
            var matchStatus = matchDisplayStatusBuilder.BuildDisplayStatus(Match);

            if (!string.IsNullOrWhiteSpace(matchStatus))
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
            if (Match == null || !Match.EventStatus.IsLive || isSubscribingTimer)
            {
                return;
            }

            eventAggregator.GetEvent<OneMinuteTimerCountUpEvent>().Subscribe(BuildMatchTime);
            isSubscribingTimer = true;
        }

        public void UnsubscribeMatchTimeChangeEvent()
        {
            eventAggregator.GetEvent<OneMinuteTimerCountUpEvent>().Unsubscribe(BuildMatchTime);
            isSubscribingTimer = false;
        }
    }
}