namespace LiveScore.Soccer.ViewModels.MatchDetailInfo
{
    using System.Collections.Generic;
    using LiveScore.Core;
    using LiveScore.Core.Enumerations;
    using LiveScore.Core.Models.Matches;
    using LiveScore.Soccer.Enumerations;
    using LiveScore.Soccer.Extensions;
    using Prism.Navigation;

    public class ScoreChangeItemViewModel : BaseItemViewModel
    {
        private static readonly IDictionary<string, string> GoalImages = new Dictionary<string, string>
        {
            { GoalMethod.OwnGoal, Images.OwnGoal.Value },
            { GoalMethod.Penalty, Images.PenaltyGoal.Value },
            { string.Empty, Images.Goal.Value },
        };

        public ScoreChangeItemViewModel(
            ITimeline timelineEvent,
            IMatchResult matchResult,
            INavigationService navigationService,
            IDependencyResolver depdendencyResolver)
             : base(timelineEvent, matchResult, navigationService, depdendencyResolver)
        {
        }

        public string HomeAssistName { get; private set; }

        public string AwayAssistName { get; private set; }

        public bool IsVisibleHomeAssist { get; private set; }

        public bool IsVisibleAwayAssist { get; private set; }

        protected override void BuildInfo()
        {
            base.BuildInfo();

            var goalMethod = TimelineEvent?.GoalScorer?.Method ?? string.Empty;
            IsVisibleHomeAssist = TimelineEvent?.Assist != null && TimelineEvent.OfHomeTeam();
            IsVisibleAwayAssist = TimelineEvent?.Assist != null && !TimelineEvent.OfHomeTeam();

            if (GoalImages.ContainsKey(goalMethod))
            {
                ImageSource = GoalImages[goalMethod];
            }

            if (TimelineEvent.OfHomeTeam())
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