namespace LiveScore.Soccer.ViewModels.MatchDetailInfo
{
    using LiveScore.Core;
    using LiveScore.Core.Models.Matches;
    using LiveScore.Soccer.Enumerations;
    using Prism.Navigation;

    public class ScoreChangeItemViewModel : BaseInfoItemViewModel
    {
        public ScoreChangeItemViewModel(
            ITimeline timelineEvent,
            IMatchResult matchResult,
            INavigationService navigationService,
            IDependencyResolver depdendencyResolver)
             : base(timelineEvent, matchResult, navigationService, depdendencyResolver)
        {
        }

        public string HomeAssistName { get; set; }

        public bool VisibleHomeBall { get; set; }

        public bool VisibleHomeOwnGoalBall { get; set; }

        public bool VisibleHomePenaltyGoalBall { get; set; }

        public string AwayAssistName { get; set; }

        public bool VisibleAwayBall { get; set; }

        public bool VisibleAwayOwnGoalBall { get; set; }

        public bool VisibleAwayPenaltyGoalBall { get; set; }

        protected override void BuildInfo()
        {
            base.BuildInfo();

            if (TimelineEvent.Team == "home")
            {
                HomePlayerName = TimelineEvent.GoalScorer?.Name;

                if (TimelineEvent.GoalScorer.Method == GoalMethod.OwnGoal)
                {
                    VisibleHomeOwnGoalBall = true;
                }
                else if (TimelineEvent.GoalScorer.Method == GoalMethod.Penalty)
                {
                    VisibleHomePenaltyGoalBall = true;
                }
                else
                {
                    HomeAssistName = TimelineEvent.Assist?.Name;
                    VisibleHomeBall = true;
                }
            }
            else
            {
                AwayPlayerName = TimelineEvent.GoalScorer?.Name;

                if (TimelineEvent.GoalScorer.Method == GoalMethod.OwnGoal)
                {
                    VisibleAwayOwnGoalBall = true;
                }
                else if (TimelineEvent.GoalScorer.Method == GoalMethod.Penalty)
                {
                    VisibleAwayPenaltyGoalBall = true;
                }
                else
                {
                    AwayAssistName = TimelineEvent.Assist?.Name;
                    VisibleAwayBall = true;
                }
            }
        }
    }
}
