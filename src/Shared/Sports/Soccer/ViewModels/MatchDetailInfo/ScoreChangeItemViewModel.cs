namespace LiveScore.Soccer.ViewModels.MatchDetailInfo
{
    using System.Collections.Generic;

    using LiveScore.Core;
    using LiveScore.Core.Models.Matches;
    using LiveScore.Soccer.Enumerations;

    using Prism.Navigation;

    public class ScoreChangeItemViewModel : BaseInfoItemViewModel
    {
        private static readonly IDictionary<string, string> GoalImages = new Dictionary<string, string>
        {
            { GoalMethod.OwnGoal, "images/common/own_goal.png" },
            { GoalMethod.Penalty, "images/common/penalty_goal.png" },
            { string.Empty, "images/common/ball.png" },
        };

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

            var goalMethod = TimelineEvent?.GoalScorer?.Method ?? string.Empty;

            if (GoalImages.ContainsKey(goalMethod))
            {
                ImageSource = GoalImages[goalMethod];
            }

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
            AwayAssistName = TimelineEvent.Assist?.Name;
            VisibleAwayImage = true;
        }

        private void BuildHomeInfo()
        {
            HomePlayerName = TimelineEvent?.GoalScorer?.Name;
            HomeAssistName = TimelineEvent.Assist?.Name;
            VisibleHomeImage = true;
        }
    }
}