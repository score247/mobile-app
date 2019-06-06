using System.Linq;
using LiveScore.Soccer.Enumerations;
namespace LiveScore.Score.ViewModels
{
    using LiveScore.Core.ViewModels;
    using LiveScore.Core;
    using Prism.Navigation;
    using LiveScore.Core.Models.Matches;
    using Xamarin.Forms;
    using LiveScore.Core.Enumerations;
    using LiveScore.Common.LangResources;

    public class MatchTimelineItemViewModel : ViewModelBase
    {
        public MatchTimelineItemViewModel(
            ITimeline timeline,
            IMatchResult matchResult,
            INavigationService navigationService,
            IDependencyResolver depdendencyResolver)
            : base(navigationService, depdendencyResolver)
        {
            Timeline = timeline;
            MatchResult = matchResult;
            VisibleMatchTime = true;
            RowColor = (Color)Application.Current.Resources["SecondColor"];

            MatchTime = string.IsNullOrEmpty(timeline.StoppageTime)
                ? timeline.MatchTime + "'"
                : timeline.MatchTime + timeline.StoppageTime + "'";

            if (timeline.Type == EventTypes.BreakStart)
            {
                VisibleMatchTime = false;
                RowColor = Color.FromHex("#2E3245");
                MainEventStatus = AppResources.HalfTime;
                VisibleMainEventStatus = true;
                VisibleScore = true;

                if (MatchResult?.MatchPeriods != null)
                {
                    var halfTimeResult = MatchResult.MatchPeriods.FirstOrDefault();
                    Score = $"{halfTimeResult.HomeScore} - {halfTimeResult.AwayScore}";
                }
                else
                {
                    Score = "-";
                }
            }

            if (timeline.Type == EventTypes.MatchEnded)
            {
                VisibleMatchTime = false;
                RowColor = Color.FromHex("#2E3245");
                MainEventStatus = AppResources.FullTime;
                VisibleMainEventStatus = true;
                VisibleScore = true;
                Score = $"{MatchResult.HomeScore} - {MatchResult.AwayScore}";
            }

            if (timeline.Type == EventTypes.PenaltyMissed)
            {
                if (timeline.Team == "home")
                {
                    VisibleHomeMissGoalBall = true;
                    HomeGoalScorerName = timeline.Player.Name;
                }
                else
                {
                    VisibleAwayMissGoalBall = true;
                    AwayGoalScorerName = timeline.Player.Name;
                }
            }

            if (timeline.Type == EventTypes.ScoreChange)
            {
                Score = $"{timeline.HomeScore} - {timeline.AwayScore}";
                VisibleScore = true;

                if (timeline.Team == "home")
                {
                    HomeGoalScorerName = timeline.GoalScorer?.Name;
                    AwayGoalScorerName = string.Empty;

                    if (timeline.GoalScorer.Method == GoalMethod.OwnGoal)
                    {
                        VisibleHomeOwnGoalBall = true;
                    }
                    else if (timeline.GoalScorer.Method == GoalMethod.Penalty)
                    {

                    }
                    else
                    {
                        VisibleHomeBall = true;
                    }
                }
                else
                {
                    AwayGoalScorerName = timeline.GoalScorer?.Name;
                    HomeGoalScorerName = string.Empty;
                    VisibleAwayBall = true;

                    if (timeline.GoalScorer.Method == GoalMethod.OwnGoal)
                    {
                        VisibleAwayOwnGoalBall = true;
                    }
                    else if (timeline.GoalScorer.Method == GoalMethod.Penalty)
                    {

                    }
                    else
                    {
                        VisibleHomeBall = true;
                    }
                }
            }

            if (timeline.Type == EventTypes.YellowCard)
            {
                if (timeline.Team == "home")
                {
                    VisibleHomeYellowCard = true;
                    HomeGoalScorerName = timeline.Player.Name;
                }
                else
                {
                    VisibleAwayYellowCard = true;
                    AwayGoalScorerName = timeline.Player.Name;
                }
            }

            if (timeline.Type == EventTypes.RedCard)
            {
                if (timeline.Team == "home")
                {
                    VisibleHomeRedCard = true;
                    HomeGoalScorerName = timeline.Player.Name;
                }
                else
                {
                    VisibleAwayRedCard = true;
                    AwayGoalScorerName = timeline.Player.Name;
                }
            }

            if (timeline.Type == EventTypes.YellowRedCard)
            {
                if (timeline.Team == "home")
                {
                    VisibleHomeRedYellowCard = true;
                    HomeGoalScorerName = timeline.Player.Name;
                }
                else
                {
                    VisibleAwayRedYellowCard = true;
                    AwayGoalScorerName = timeline.Player.Name;
                }
            }
        }

        public ITimeline Timeline { get; }

        public IMatchResult MatchResult { get; }

        public Color RowColor { get; set; }

        public string MainEventStatus { get; set; }

        public bool VisibleMainEventStatus { get; set; }

        public string MatchTime { get; }

        public bool VisibleMatchTime { get; set; }

        public string HomeGoalScorerName { get; set; }

        public bool VisibleHomeBall { get; set; }

        public bool VisibleHomeOwnGoalBall { get; set; }

        public bool VisibleHomeYellowCard { get; set; }

        public bool VisibleHomeRedCard { get; set; }

        public bool VisibleHomeRedYellowCard { get; set; }

        public bool VisibleHomeMissGoalBall { get; set; }

        public bool VisibleScore { get; set; }

        public string Score { get; set; }

        public string AwayGoalScorerName { get; set; }

        public bool VisibleAwayBall { get; set; }

        public bool VisibleAwayOwnGoalBall { get; set; }

        public bool VisibleAwayYellowCard { get; set; }

        public bool VisibleAwayRedCard { get; set; }

        public bool VisibleAwayRedYellowCard { get; set; }

        public bool VisibleAwayMissGoalBall { get; set; }
    }
}

