namespace LiveScore.Soccer.ViewModels.MatchDetailInfo
{
    using System.Linq;
    using LiveScore.Common.LangResources;
    using LiveScore.Core;
    using LiveScore.Core.Models.Matches;
    using LiveScore.Soccer.Extensions;
    using Prism.Navigation;
    using Xamarin.Forms;

    public class MainEventItemViewModel : BaseItemViewModel
    {
        public MainEventItemViewModel(
            ITimeline timelineEvent,
            IMatchResult matchResult,
            INavigationService navigationService,
            IDependencyResolver dependencyResolver)
             : base(timelineEvent, matchResult, navigationService, dependencyResolver)
        {
        }

        public string MainEventStatus { get; protected set; }

        protected override void BuildInfo()
        {
            base.BuildInfo();

            if (Application.Current != null)
            {
                RowColor = (Color)Application.Current.Resources["FourthAccentColor"];
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

            if (IsAfterExtraTime())
            {
                BuildAfterExtraTime();
                return;
            }

            if (TimelineEvent.IsPenaltyShootOutStart())
            {
                BuildPenaltyShootOut();
            }
        }

        private void BuildHalfTime()
        {
            MainEventStatus = AppResources.HalfTime;

            if (Result?.MatchPeriods != null)
            {
                var result = Result.MatchPeriods?.FirstOrDefault();
                Score = $"{result?.HomeScore} - {result?.AwayScore}";
            }
        }

        private void BuildFullTime()
        {
            MainEventStatus = AppResources.FullTime;

            if (Result.HasFullTimeResult())
            {
                var firstHalfResult = Result.MatchPeriods.ToList()[0];
                var secondHalfResult = Result.MatchPeriods.ToList()[1];
                var totalHomeScore = firstHalfResult?.HomeScore + secondHalfResult?.HomeScore;
                var totalAwayScore = firstHalfResult?.AwayScore + secondHalfResult?.AwayScore;
                Score = $"{totalHomeScore} - {totalAwayScore}";
            }
        }

        private void BuildAfterExtraTime()
        {
            MainEventStatus = AppResources.AfterExtraTime;
            Score = $"{Result?.HomeScore} - {Result?.AwayScore}";
        }

        private void BuildPenaltyShootOut()
        {
            MainEventStatus = AppResources.PenaltyShootOut;
            var penaltyScore = Result?.GetPenaltyResult();
            Score = $"{penaltyScore?.HomeScore} - {penaltyScore?.AwayScore}";
        }

        private bool IsFullTime()
            => TimelineEvent.IsAwaitingExtraTimeBreak()
                || (TimelineEvent.IsAwaitingPenaltiesBreak() && Result.GetOvertimeResult() == null)
                || (TimelineEvent.IsMatchEndedFullTime(Result));

        private bool IsAfterExtraTime()
            => (TimelineEvent.IsAwaitingPenaltiesBreak() && Result.GetOvertimeResult() != null)
                || TimelineEvent.IsMatchEndedExtraTime(Result);
    }
}