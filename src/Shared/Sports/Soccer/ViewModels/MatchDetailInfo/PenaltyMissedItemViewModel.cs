namespace LiveScore.Soccer.ViewModels.MatchDetailInfo
{
    using LiveScore.Core;
    using LiveScore.Core.Models.Matches;
    using Prism.Navigation;

    public class PenaltyMissedItemViewModel : BaseInfoItemViewModel
    {
        public PenaltyMissedItemViewModel(
            ITimeline timelineEvent,
            IMatchResult matchResult,
            INavigationService navigationService,
            IDependencyResolver depdendencyResolver)
             : base(timelineEvent, matchResult, navigationService, depdendencyResolver)
        {
        }

        public bool VisibleHomeMissPenaltyGoalBall { get; protected set; }

        public bool VisibleAwayMissPenaltyGoalBall { get; protected set; }

        protected override void BuildInfo()
        {
            base.BuildInfo();

            if (TimelineEvent.Team == "home")
            {
                VisibleHomeMissPenaltyGoalBall = true;
                HomePlayerName = TimelineEvent?.Player?.Name;
            }
            else
            {
                VisibleAwayMissPenaltyGoalBall = true;
                AwayPlayerName = TimelineEvent?.Player?.Name;
            }
        }
    }
}