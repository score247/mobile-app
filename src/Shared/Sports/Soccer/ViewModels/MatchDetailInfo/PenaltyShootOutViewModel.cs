namespace LiveScore.Soccer.ViewModels.MatchDetailInfo
{
    using LiveScore.Core;
    using LiveScore.Core.Enumerations;
    using LiveScore.Core.Models.Matches;
    using Prism.Navigation;

    public class PenaltyShootOutViewModel : BaseItemViewModel
    {
        public PenaltyShootOutViewModel(

            ITimeline timelineEvent,
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
            var homeScore = "  ";
            var awayScore = "  ";

            if (TimelineEvent.HomeShootoutPlayer != null)
            {
                HomeImageSource = TimelineEvent.IsHomeShootoutScored ?
                    Images.PenaltyGoal.Value :
                    Images.MissPenaltyGoal.Value;

                HomePlayerName = TimelineEvent.HomeShootoutPlayer?.Name;
                homeScore = TimelineEvent.ShootoutHomeScore.ToString();
            }

            if (TimelineEvent.AwayShootoutPlayer != null)
            {
                AwayImageSource = TimelineEvent.IsAwayShootoutScored ?
                Images.PenaltyGoal.Value :
                Images.MissPenaltyGoal.Value;

                AwayPlayerName = TimelineEvent.AwayShootoutPlayer?.Name;
                awayScore = TimelineEvent.ShootoutAwayScore.ToString();
            }

            Score = $"{homeScore} - {awayScore}";
        }
    }
}