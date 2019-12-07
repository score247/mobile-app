using LiveScore.Core;
using LiveScore.Soccer.Enumerations;
using LiveScore.Soccer.Extensions;
using LiveScore.Soccer.Models.Matches;
using LiveScore.Soccer.Models.TimelineImages;
using Prism.Navigation;

namespace LiveScore.Soccer.ViewModels.Matches.MatchDetails.Information.InfoItems
{
    public class ScoreChangeItemViewModel : BaseItemViewModel
    {
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

        public override BaseItemViewModel BuildData()
        {
            base.BuildData();

            IsVisibleHomeAssist = TimelineEvent?.Assist != null && TimelineEvent.OfHomeTeam();
            IsVisibleAwayAssist = TimelineEvent?.Assist != null && !TimelineEvent.OfHomeTeam();

            ImageSource = ImageConverter.BuildImageSource(
                new TimelineEventImage(TimelineEvent?.Type, TimelineEvent?.GoalScorer));

            if (TimelineEvent.OfHomeTeam())
            {
                BuildHomeInfo();
            }
            else
            {
                BuildAwayInfo();
            }

            return this;
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