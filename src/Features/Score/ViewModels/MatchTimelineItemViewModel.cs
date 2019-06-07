﻿// <auto-generated>
// </auto-generated>
namespace LiveScore.Score.ViewModels
{
    using LiveScore.Common.LangResources;
    using LiveScore.Core;
    using LiveScore.Core.Enumerations;
    using LiveScore.Core.Models.Matches;
    using LiveScore.Core.ViewModels;
    using Prism.Navigation;
    using Xamarin.Forms;
    using System.Linq;
    using LiveScore.Soccer.Enumerations;

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

            BuildData(timeline);
        }

        private void BuildData(ITimeline timeline)
        {
            Score = $"{timeline.HomeScore} - {timeline.AwayScore}";

            BuildMatchTime(timeline);
            BuildBreakStart(timeline);
            BuildMatchEnd(timeline);
            BuildScoreChange(timeline);
            BuildCard(timeline);
        }

        private void BuildMatchTime(ITimeline timeline)
        {
            RowColor = (Color)Application.Current.Resources["SecondColor"];
            VisibleMatchTime = true;

            MatchTime = string.IsNullOrEmpty(timeline.StoppageTime)
                ? timeline.MatchTime + "'"
                : timeline.MatchTime + "+" + timeline.StoppageTime + "'";
        }

        private void BuildCard(ITimeline timeline)
        {
            if (timeline.Type == EventTypes.YellowCard)
            {
                if (timeline.Team == "home")
                {
                    VisibleHomeYellowCard = true;
                    HomePlayerName = timeline.Player.Name;
                }
                else
                {
                    VisibleAwayYellowCard = true;
                    AwayPlayerName = timeline.Player.Name;
                }
            }

            if (timeline.Type == EventTypes.RedCard)
            {
                if (timeline.Team == "home")
                {
                    VisibleHomeRedCard = true;
                    HomePlayerName = timeline.Player.Name;
                }
                else
                {
                    VisibleAwayRedCard = true;
                    AwayPlayerName = timeline.Player.Name;
                }
            }

            if (timeline.Type == EventTypes.YellowRedCard)
            {
                if (timeline.Team == "home")
                {
                    VisibleHomeRedYellowCard = true;
                    HomePlayerName = timeline.Player.Name;
                }
                else
                {
                    VisibleAwayRedYellowCard = true;
                    AwayPlayerName = timeline.Player.Name;
                }
            }
        }

        private void BuildScoreChange(ITimeline timeline)
        {
            if (timeline.Type == EventTypes.ScoreChange)
            {
                VisibleScore = true;

                if (timeline.Team == "home")
                {
                    HomePlayerName = timeline.GoalScorer?.Name;

                    if (timeline.GoalScorer.Method == GoalMethod.OwnGoal)
                    {
                        VisibleHomeOwnGoalBall = true;
                    }
                    else if (timeline.GoalScorer.Method == GoalMethod.Penalty)
                    {
                        VisibleHomePenaltyGoalBall = true;
                    }
                    else
                    {
                        HomeAssistName = timeline.Assist?.Name;
                        VisibleHomeBall = true;
                    }
                }
                else
                {
                    AwayPlayerName = timeline.GoalScorer?.Name;

                    if (timeline.GoalScorer.Method == GoalMethod.OwnGoal)
                    {
                        VisibleAwayOwnGoalBall = true;
                    }
                    else if (timeline.GoalScorer.Method == GoalMethod.Penalty)
                    {
                        VisibleAwayPenaltyGoalBall = true;
                    }
                    else
                    {
                        AwayAssistName = timeline.Assist?.Name;
                        VisibleAwayBall = true;
                    }
                }
            }
        }

        private void BuildMatchEnd(ITimeline timeline)
        {
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
                VisibleScore = true;

                if (timeline.Team == "home")
                {
                    VisibleHomeMissPenaltyGoalBall = true;
                    HomePlayerName = timeline.Player.Name;
                }
                else
                {
                    VisibleAwayMissPenaltyGoalBall = true;
                    AwayPlayerName = timeline.Player.Name;
                }
            }
        }

        private void BuildBreakStart(ITimeline timeline)
        {
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
        }

        public ITimeline Timeline { get; }

        public IMatchResult MatchResult { get; }

        public Color RowColor { get; private set; }

        public string MainEventStatus { get; private set; }

        public bool VisibleMainEventStatus { get; private set; }

        public string MatchTime { get; private set; }

        public bool VisibleMatchTime { get; private set; }

        public string HomePlayerName { get; set; }

        public string HomeAssistName { get; set; }

        public bool VisibleHomeBall { get; set; }

        public bool VisibleHomeOwnGoalBall { get; set; }

        public bool VisibleHomeYellowCard { get; set; }

        public bool VisibleHomeRedCard { get; set; }

        public bool VisibleHomeRedYellowCard { get; set; }

        public bool VisibleHomeMissPenaltyGoalBall { get; set; }

        public bool VisibleHomePenaltyGoalBall { get; set; }

        public bool VisibleScore { get; set; }

        public string Score { get; set; }

        public string AwayPlayerName { get; set; }

        public string AwayAssistName { get; set; }

        public bool VisibleAwayBall { get; set; }

        public bool VisibleAwayOwnGoalBall { get; set; }

        public bool VisibleAwayYellowCard { get; set; }

        public bool VisibleAwayRedCard { get; set; }

        public bool VisibleAwayRedYellowCard { get; set; }

        public bool VisibleAwayMissPenaltyGoalBall { get; set; }

        public bool VisibleAwayPenaltyGoalBall { get; set; }
    }
}