namespace LiveScore.Soccer.ViewModels
{
    using LiveScore.Core;
    using LiveScore.Core.Models.Matches;
    using LiveScore.Soccer.Enumerations;
    using Prism.Navigation;

    public class ScoreChangeItemViewModel : MatchDetailInfoItemViewModel
    {
        public ScoreChangeItemViewModel(
            ITimeline timelineEvent,
            IMatchResult matchResult,
            INavigationService navigationService,
            IDependencyResolver depdendencyResolver)
             : base(timelineEvent, matchResult, navigationService, depdendencyResolver)
        {
            VisibleScore = true;

            if (timelineEvent.Team == "home")
            {
                HomePlayerName = timelineEvent.GoalScorer?.Name;

                if (timelineEvent.GoalScorer.Method == GoalMethod.OwnGoal)
                {
                    VisibleHomeOwnGoalBall = true;
                }
                else if (timelineEvent.GoalScorer.Method == GoalMethod.Penalty)
                {
                    VisibleHomePenaltyGoalBall = true;
                }
                else
                {
                    HomeAssistName = timelineEvent.Assist?.Name;
                    VisibleHomeBall = true;
                }
            }
            else
            {
                AwayPlayerName = timelineEvent.GoalScorer?.Name;

                if (timelineEvent.GoalScorer.Method == GoalMethod.OwnGoal)
                {
                    VisibleAwayOwnGoalBall = true;
                }
                else if (timelineEvent.GoalScorer.Method == GoalMethod.Penalty)
                {
                    VisibleAwayPenaltyGoalBall = true;
                }
                else
                {
                    AwayAssistName = timelineEvent.Assist?.Name;
                    VisibleAwayBall = true;
                }
            }
        }
    }
}
