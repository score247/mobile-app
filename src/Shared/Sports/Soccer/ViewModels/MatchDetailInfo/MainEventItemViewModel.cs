namespace LiveScore.Soccer.ViewModels.MatchDetailInfo
{
    using System.Linq;
    using LiveScore.Common.LangResources;
    using LiveScore.Core;
    using LiveScore.Core.Enumerations;
    using LiveScore.Core.Models.Matches;
    using Prism.Navigation;
    using Xamarin.Forms;

    public class MainEventItemViewModel : BaseItemViewModel
    {
        private const int NumberOfFullTimePeriodsResult = 2;

        public MainEventItemViewModel(
            ITimeline timelineEvent,
            IMatchResult matchResult,
            INavigationService navigationService,
            IDependencyResolver depdendencyResolver)
             : base(timelineEvent, matchResult, navigationService, depdendencyResolver)
        {
        }

        public string MainEventStatus { get; protected set; }

        protected override void BuildInfo()
        {
            base.BuildInfo();

            if (Application.Current != null)
            {
                RowColor = (Color)Application.Current.Resources["LineColor"];
            }

            Score = "-";

            if (IsHalfTime())
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

            if (IsPenaltyShootOut())
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

            if (Result?.MatchPeriods != null && Result.MatchPeriods.Count() >= NumberOfFullTimePeriodsResult)
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
            var penaltyScore = Result.MatchPeriods?.FirstOrDefault(p => p.PeriodType?.IsPenalties == true);
            Score = $"{penaltyScore?.HomeScore} - {penaltyScore?.AwayScore}";
        }

        private bool IsHalfTime() => TimelineEvent.Type == EventTypes.BreakStart && TimelineEvent.PeriodType == PeriodTypes.Pause;

        private bool IsFullTime()
            => (TimelineEvent.Type == EventTypes.BreakStart && TimelineEvent.PeriodType == PeriodTypes.AwaitingExtraTime)
                || (TimelineEvent.Type == EventTypes.MatchEnded && Result.MatchStatus.IsEnded);

        private bool IsAfterExtraTime()
            => (TimelineEvent.Type == EventTypes.BreakStart && TimelineEvent.PeriodType == PeriodTypes.AwaitingPenalties)
              || (TimelineEvent.Type == EventTypes.MatchEnded && Result.MatchStatus.IsAfterExtraTime);

        private bool IsPenaltyShootOut()
            => TimelineEvent.Type == EventTypes.PeriodStart && TimelineEvent.PeriodType == PeriodTypes.Penalties;
    }
}