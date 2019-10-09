namespace LiveScore.Soccer.ViewModels.MatchDetailInfo
{
    using LiveScore.Common.LangResources;
    using LiveScore.Core;
    using LiveScore.Core.Models.Matches;
    using LiveScore.Soccer.Extensions;
    using LiveScore.Soccer.Models.Matches;
    using Prism.Navigation;
    using Xamarin.Forms;

    public class MainEventItemViewModel : BaseItemViewModel
    {
        public MainEventItemViewModel(
            TimelineEvent timelineEvent,
            MatchInfo matchInfo,
            INavigationService navigationService,
            IDependencyResolver dependencyResolver)
             : base(timelineEvent, matchInfo, navigationService, dependencyResolver)
        {
        }

        public string MainEventStatus { get; protected set; }

        protected override void BuildInfo()
        {
            base.BuildInfo();

            if (Application.Current != null)
            {
                RowColor = (Color)Application.Current.Resources["FunctionGroupBgColor"];
            }

            Score = "-";

            if (TimelineEvent.IsHalfTimeBreak())
            {
                BuildHalfTime();
                return;
            }

            if (IsFullTime())
            {
                BuildFullTime();
                return;
            }

            if (TimelineEvent.IsExtraTimeHalfTimeBreak())
            {
                BuildExtraTimeHalfTime();
                return;
            }

            if (IsAfterExtraTime())
            {
                BuildAfterExtraTime();
                return;
            }

            if (TimelineEvent.IsMatchEndAfterPenalty(MatchInfo.Match))
            {
                BuildPenaltyShootOut();
            }
        }

        private void BuildHalfTime()
        {
            MainEventStatus = AppResources.HalfTime;

            Score = $"{TimelineEvent?.HomeScore} - {TimelineEvent?.AwayScore}";
        }

        private void BuildExtraTimeHalfTime()
        {
            MainEventStatus = AppResources.ExtraTimeHalfTime;

            Score = $"{TimelineEvent.HomeScore} - {TimelineEvent.AwayScore}";
        }

        private void BuildFullTime()
        {
            MainEventStatus = AppResources.FullTime;

            Score = $"{TimelineEvent.HomeScore} - {TimelineEvent.AwayScore}";
        }

        private void BuildAfterExtraTime()
        {
            MainEventStatus = AppResources.AfterExtraTime;

            Score = $"{TimelineEvent?.HomeScore} - {TimelineEvent?.AwayScore}";
        }

        private void BuildPenaltyShootOut()
        {
            MainEventStatus = AppResources.PenaltyShootOut;
            var penaltyScore = Match?.GetPenaltyResult();
            Score = $"{penaltyScore?.HomeScore} - {penaltyScore?.AwayScore}";
        }

        private bool IsFullTime()
            => TimelineEvent.IsAwaitingExtraTimeBreak()
                || (TimelineEvent.IsAwaitingPenaltiesBreak() && Match.GetOvertimeResult() == null)
                || (TimelineEvent.IsMatchEndedFullTime(Match));

        private bool IsAfterExtraTime()
            => (TimelineEvent.IsAwaitingPenaltiesBreak() && Match.GetOvertimeResult() != null)
                || TimelineEvent.IsMatchEndedExtraTime(Match);
    }
}