using System.Collections.Generic;

namespace LiveScore.Core.Enumerations
{
    public class EventTypes : Enumeration
    {
        //break_start
        public static readonly EventTypes BreakStart = new EventTypes(1, "break_start");

        //cancelled_video_assistant_referee
        //corner_kick
        //free_kick
        //goal_kick
        //injury
        //injury_return

        //injury_time_shown
        public static readonly EventTypes InjuryTimeShown = new EventTypes(2, "injury_time_shown");

        //match_ended
        public static readonly EventTypes MatchEnded = new EventTypes(3, "match_ended");

        //match_started
        public static readonly EventTypes MatchStarted = new EventTypes(4, "match_started");

        //offside
        //penalty_awarded
        //penalty_missed
        public static readonly EventTypes PenaltyMissed = new EventTypes(5, "penalty_missed");

        //penalty_shootout
        public static readonly EventTypes PenaltyShootout = new EventTypes(6, "penalty_shootout");

        //period_score

        //period_start
        public static readonly EventTypes PeriodStart = new EventTypes(7, "period_start");

        //possible_video_assistant_referee
        //red_card
        public static readonly EventTypes RedCard = new EventTypes(8, "red_card");

        //score_change
        public static readonly EventTypes ScoreChange = new EventTypes(9, "score_change");

        //shot_off_target
        //shot_on_target
        //shot_saved
        //substitution
        public static readonly EventTypes Substitution = new EventTypes(10, "substitution");

        //throw_in
        //video_assistant_referee
        //video_assistant_referee_over
        //yellow_card
        public static readonly EventTypes YellowCard = new EventTypes(11, "yellow_card");

        //yellow_red_card
        public static readonly EventTypes YellowRedCard = new EventTypes(12, "yellow_red_card");

        public EventTypes()
        {
        }

        public static List<string> BasicSoccerEventTypes
           => new List<string>
           {
                MatchStarted.DisplayName,
                PeriodStart.DisplayName,
                BreakStart.DisplayName,
                ScoreChange.DisplayName,
                YellowCard.DisplayName,
                YellowRedCard.DisplayName,
                RedCard.DisplayName,
                PenaltyShootout.DisplayName
           };

        private EventTypes(byte value, string displayName)
            : base(value, displayName)
        {
        }

        public bool IsInjuryTimeShown => this == InjuryTimeShown;
    }
}