namespace LiveScore.Soccer.ViewModels.MatchDetailInfo
{
    using System.Collections;
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

        public string AwayAssistName { get; set; }

        public string ImageSource { get; private set; }

        public bool VisibleHomeImage { get; private set; }

        public bool VisibleAwayImage { get; private set; }

        protected override void BuildInfo()
        {
            base.BuildInfo();

            if (TimelineEvent?.Team == "home")
            {
                BuildHomeInfo();
            }
            else
            {
                BuildAwayInfo();
            }
        }

        private void BuildAwayInfo()
        {
            AwayPlayerName = TimelineEvent?.GoalScorer?.Name;
            VisibleAwayImage = true;

            if (TimelineEvent?.GoalScorer?.Method == GoalMethod.OwnGoal)
            {
                ImageSource = "images/common/own_goal.png";
            }
            else if (TimelineEvent?.GoalScorer?.Method == GoalMethod.Penalty)
            {
                ImageSource = "images/common/penalty_goal.png";
            }
            else
            {
                AwayAssistName = TimelineEvent.Assist?.Name;
                ImageSource = "images/common/ball.png";
            }
        }

        private void BuildHomeInfo()
        {
            HomePlayerName = TimelineEvent?.GoalScorer?.Name;
            VisibleHomeImage = true;

            if (TimelineEvent?.GoalScorer?.Method == GoalMethod.OwnGoal)
            {
                ImageSource = "images/common/own_goal.png";
            }
            else if (TimelineEvent?.GoalScorer?.Method == GoalMethod.Penalty)
            {
                ImageSource = "images/common/penalty_goal.png";
            }
            else
            {
                HomeAssistName = TimelineEvent.Assist?.Name;
                ImageSource = "images/common/ball.png";
            }
        }
    }
}