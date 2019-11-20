using LiveScore.Common.LangResources;
using LiveScore.Core;
using LiveScore.Soccer.Extensions;
using LiveScore.Soccer.Models.Matches;
using Prism.Navigation;
using Xamarin.Forms;

namespace LiveScore.Soccer.ViewModels.MatchDetails.Information.InfoItems
{
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

        public override BaseItemViewModel BuildData()
        {
            base.BuildData();

            if (Application.Current != null)
            {
                RowColor = (Color)Application.Current.Resources["FunctionGroupBgColor"];
            }

            Score = "-";

            if (TimelineEvent.IsHalfTimeBreak())
            {
                BuildHalfTime();
                return this;
            }

            if (IsFullTime())
            {
                BuildFullTime();
                return this;
            }

            if (TimelineEvent.IsExtraTimeHalfTimeBreak())
            {
                BuildExtraTimeHalfTime();
                return this;
            }

            if (IsAfterExtraTime())
            {
                BuildAfterExtraTime();
                return this;
            }

            if (TimelineEvent.IsMatchEndAfterPenalty(MatchInfo.Match))
            {
                BuildPenaltyShootOut();
            }

            return this;
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