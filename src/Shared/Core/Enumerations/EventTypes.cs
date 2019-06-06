using System.Collections.Generic;

namespace LiveScore.Core.Enumerations
{
    public class EventTypes : Enumeration
    {
        //break_start
        public const string BreakStart = "break_start";

        public static readonly EventTypes BreakStartType = new EventTypes(BreakStart, nameof(BreakStart));

        public const string MatchEnded = "match_ended";

        public static readonly EventTypes MatchEndedType = new EventTypes(MatchEnded, nameof(MatchEnded));

        public const string MatchStarted = "match_started";

        public static readonly EventTypes MatchStartedType = new EventTypes(MatchStarted, nameof(MatchStarted));

        public const string PeriodStart = "period_start";

        public static readonly EventTypes PeriodStartType = new EventTypes(PeriodStart, nameof(PeriodStart));

        public const string PenaltyShootout = "penalty_shootout";

        public static readonly EventTypes PenaltyShootoutType = new EventTypes(PenaltyShootout, nameof(PenaltyShootout));

        public const string PenaltyMissed = "penalty_missed";

        public static readonly EventTypes PenaltyMissedType = new EventTypes(PenaltyMissed, nameof(PenaltyMissed));
        //cancelled_video_assistant_referee
        //corner_kick
        //free_kick
        //goal_kick
        //injury
        //injury_return
        //injury_time_shown
        public const string InjuryTimeShown = "injury_time_shown";

        public static readonly EventTypes InjuryTimeShownType = new EventTypes(InjuryTimeShown, nameof(InjuryTimeShown));

        //match_ended
        //match_started
        //offside
        //penalty_awarded
        //penalty_missed
        //penalty_shootout
        //period_score
        //period_start
        //possible_video_assistant_referee
        //red_card
        public const string RedCard = "red_card";

        public static readonly EventTypes RedCardType = new EventTypes(RedCard, nameof(RedCard));

        //score_change
        public const string ScoreChange = "score_change";

        public static readonly EventTypes ScoreChangeType = new EventTypes(ScoreChange, nameof(ScoreChange));

        //shot_off_target
        //shot_on_target
        //shot_saved
        //substitution
        public const string Substitution = "substitution";

        public static readonly EventTypes SubstitutionType = new EventTypes(Substitution, nameof(Substitution));

        //throw_in
        //video_assistant_referee
        //video_assistant_referee_over
        //yellow_card
        public const string YellowCard = "yellow_card";

        public static readonly EventTypes YellowCardType = new EventTypes(YellowCard, nameof(YellowCard));

        //yellow_red_card
        public const string YellowRedCard = "yellow_red_card";

        public static readonly EventTypes YellowRedCardType = new EventTypes(YellowRedCard, nameof(YellowRedCard));

        public EventTypes()
        {
        }

        public static List<string> BasicSoccerEventTypes
           => new List<string>
           {
                MatchStarted,
                PeriodStart,
                BreakStart,
                ScoreChange,
                YellowCard,
                YellowRedCard,
                RedCard,
                PenaltyShootout
           };

        private EventTypes(string value, string displayName)
            : base(value, displayName)
        {
        }

        public bool IsInjuryTimeShown => Value == InjuryTimeShown;
    }
}