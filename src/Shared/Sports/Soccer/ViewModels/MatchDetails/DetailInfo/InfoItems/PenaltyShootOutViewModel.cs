namespace LiveScore.Soccer.ViewModels.MatchDetailInfo
{
    using LiveScore.Core;
    using LiveScore.Core.Models.Matches;
    using LiveScore.Soccer.Enumerations;
    using Prism.Navigation;

    public class PenaltyShootOutViewModel : BaseItemViewModel
    {
        public PenaltyShootOutViewModel(

            ITimelineEvent timelineEvent,
            IMatchResult matchResult,
            INavigationService navigationService,
            IDependencyResolver depdendencyResolver)
             : base(timelineEvent, matchResult, navigationService, depdendencyResolver)
        {
        }

        public string HomeImageSource { get; private set; }

        public string AwayImageSource { get; private set; }

        protected override void BuildInfo()
        {
            base.BuildInfo();
            var homeScore = TimelineEvent.ShootoutHomeScore;
            var awayScore = TimelineEvent.ShootoutAwayScore;

            if (TimelineEvent.HomeShootoutPlayer != null)
            {
                HomeImageSource = TimelineEvent.IsHomeShootoutScored ?
                    Images.PenaltyGoal.Value :
                    Images.MissPenaltyGoal.Value;

                HomePlayerName = TimelineEvent.HomeShootoutPlayer?.Name;
            }

            if (TimelineEvent.AwayShootoutPlayer != null)
            {
                AwayImageSource = TimelineEvent.IsAwayShootoutScored ?
                    Images.PenaltyGoal.Value :
                    Images.MissPenaltyGoal.Value;

                AwayPlayerName = TimelineEvent.AwayShootoutPlayer?.Name;
            }

            Score = $"{homeScore} - {awayScore}";
        }
    }
}