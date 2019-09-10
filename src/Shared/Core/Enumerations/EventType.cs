namespace LiveScore.Core.Enumerations
{
    using System.Collections.Generic;
    using MessagePack;

    [MessagePackObject]
    public class EventType : Enumeration
    {
        //break_start
        public static readonly EventType BreakStart = new EventType(1, "break_start");

        //cancelled_video_assistant_referee
        //corner_kick
        //free_kick
        //goal_kick
        //injury
        //injury_return

        //injury_time_shown
        public static readonly EventType InjuryTimeShown = new EventType(2, "injury_time_shown");

        //match_ended
        public static readonly EventType MatchEnded = new EventType(3, "match_ended");

        //match_started
        public static readonly EventType MatchStarted = new EventType(4, "match_started");

        //offside
        //penalty_awarded
        //penalty_missed
        public static readonly EventType PenaltyMissed = new EventType(5, "penalty_missed");

        //penalty_shootout
        public static readonly EventType PenaltyShootout = new EventType(6, "penalty_shootout");

        //period_score

        //period_start
        public static readonly EventType PeriodStart = new EventType(7, "period_start");

        //possible_video_assistant_referee
        //red_card
        public static readonly EventType RedCard = new EventType(8, "red_card");

        //score_change
        public static readonly EventType ScoreChange = new EventType(9, "score_change");

        //shot_off_target
        //shot_on_target
        //shot_saved
        //substitution
        public static readonly EventType Substitution = new EventType(10, "substitution");

        //throw_in
        //video_assistant_referee
        //video_assistant_referee_over
        //yellow_card
        public static readonly EventType YellowCard = new EventType(11, "yellow_card");

        //yellow_red_card
        public static readonly EventType YellowRedCard = new EventType(12, "yellow_red_card");

        public EventType()
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

        private EventType(byte value, string displayName)
            : base(value, displayName)
        {
        }

        [IgnoreMember]
        public bool IsInjuryTimeShown => this == InjuryTimeShown;

        public bool IsRedCard() => this == RedCard || this == YellowRedCard;

        [IgnoreMember]
        public bool IsMatchStarted => this == MatchStarted;

        [IgnoreMember]
        public bool IsMatchEnded => this == MatchEnded;

        [IgnoreMember]
        public bool IsPeriodStart => this == PeriodStart;
    }
}