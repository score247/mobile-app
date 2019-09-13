namespace LiveScore.Soccer.ViewModels.MatchDetailInfo
{
    using System.Collections.Generic;
    using LiveScore.Core;
    using LiveScore.Core.Models.Matches;
    using LiveScore.Soccer.Enumerations;
    using LiveScore.Soccer.Extensions;
    using LiveScore.Soccer.Models.Matches;
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
            TimelineEvent timelineEvent,
            MatchInfo matchInfo,
            INavigationService navigationService,
            IDependencyResolver dependencyResolver)
             : base(timelineEvent, matchInfo, navigationService, dependencyResolver)
        {
        }

        public string HomeAssistName { get; private set; }

        public string AwayAssistName { get; private set; }

        public bool IsVisibleHomeAssist { get; private set; }

        public bool IsVisibleAwayAssist { get; private set; }

        public string GoalAssistImageSource { get; private set; }

        protected override void BuildInfo()
        {
            base.BuildInfo();

            var goalMethod = TimelineEvent?.GoalScorer?.Method ?? string.Empty;
            IsVisibleHomeAssist = TimelineEvent?.Assist != null && TimelineEvent.OfHomeTeam();
            IsVisibleAwayAssist = TimelineEvent?.Assist != null && !TimelineEvent.OfHomeTeam();

            ImageSource = GoalImages.ContainsKey(goalMethod)
                ? GoalImages[goalMethod]
                : Images.Goal.Value; // Have case header

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

            if (TimelineEvent?.Assist != null)
            {
                AwayAssistName = TimelineEvent.Assist.Name;
                GoalAssistImageSource = Images.GoalAssist.Value;
            }

            VisibleAwayImage = true;
        }

        private void BuildHomeInfo()
        {
            HomePlayerName = TimelineEvent?.GoalScorer?.Name;

            if (TimelineEvent?.Assist != null)
            {
                HomeAssistName = TimelineEvent.Assist.Name;
                GoalAssistImageSource = Images.GoalAssist.Value;
            }

            VisibleHomeImage = true;
        }
    }
}