namespace LiveScore.Soccer.ViewModels.MatchDetailInfo
{
    using LiveScore.Core;
    using LiveScore.Core.Models.Matches;
    using LiveScore.Soccer.Enumerations;
    using LiveScore.Soccer.Models.Matches;
    using Prism.Navigation;

    public class PenaltyShootOutViewModel : BaseItemViewModel
    {
        public PenaltyShootOutViewModel(
            TimelineEvent timelineEvent,
            MatchInfo matchInfo,
            INavigationService navigationService,
            IDependencyResolver depdendencyResolver)
             : base(timelineEvent, matchInfo, navigationService, depdendencyResolver)
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
                    Images.PenaltyShootoutGoal.Value :
                    Images.MissedPenaltyShootoutGoal.Value;

                HomePlayerName = TimelineEvent.HomeShootoutPlayer?.Name;
            }

            if (TimelineEvent.AwayShootoutPlayer != null)
            {
                AwayImageSource = TimelineEvent.IsAwayShootoutScored ?
                    Images.PenaltyShootoutGoal.Value :
                    Images.MissedPenaltyShootoutGoal.Value;

                AwayPlayerName = TimelineEvent.AwayShootoutPlayer?.Name;
            }

            Score = $"{homeScore} - {awayScore}";
        }
    }
}