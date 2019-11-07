namespace LiveScore.Core.Enumerations
{
    using System.Collections.Generic;
    using MessagePack;

    [MessagePackObject]
    public class EventType : Enumeration
    {
        //break_start
        public static readonly EventType BreakStart = new EventType(1, "break_start");

        //injury_time_shown
        public static readonly EventType InjuryTimeShown = new EventType(2, "injury_time_shown");

        //match_ended
        public static readonly EventType MatchEnded = new EventType(3, "match_ended");

        //match_started
        public static readonly EventType MatchStarted = new EventType(4, "match_started");

        //penalty_missed
        public static readonly EventType PenaltyMissed = new EventType(5, "penalty_missed");

        //penalty_shootout
        public static readonly EventType PenaltyShootout = new EventType(6, "penalty_shootout");

        //period_start
        public static readonly EventType PeriodStart = new EventType(7, "period_start");

        //red_card
        public static readonly EventType RedCard = new EventType(8, "red_card");

        //score_change
        public static readonly EventType ScoreChange = new EventType(9, "score_change");

        //substitution
        public static readonly EventType Substitution = new EventType(10, "substitution");

        //yellow_card
        public static readonly EventType YellowCard = new EventType(11, "yellow_card");

        //yellow_red_card
        public static readonly EventType YellowRedCard = new EventType(12, "yellow_red_card");

        //cancelled_video_assistant_referee
        public static readonly EventType CancelledVideoAssistantReferee = new EventType(13, "cancelled_video_assistant_referee");

        //corner_kick
        public static readonly EventType CornerKick = new EventType(14, "corner_kick");

        //free_kick
        public static readonly EventType FreeKick = new EventType(15, "free_kick");

        //goal_kick
        public static readonly EventType GoalKick = new EventType(16, "goal_kick");

        //injury
        public static readonly EventType Injury = new EventType(17, "injury");

        //injury_return
        public static readonly EventType InjuryReturn = new EventType(18, "injury_return");

        //shot_off_target
        public static readonly EventType ShotOffTarget = new EventType(19, "shot_off_target");

        //shot_on_target
        public static readonly EventType ShotOnTarget = new EventType(20, "shot_on_target");

        //shot_saved
        public static readonly EventType ShotSaved = new EventType(21, "shot_saved");

        //throw_in
        public static readonly EventType ThrowIn = new EventType(22, "throw_in");

        //video_assistant_referee
        public static readonly EventType VideoAssistantReferee = new EventType(23, "video_assistant_referee");

        //video_assistant_referee_over
        public static readonly EventType VideoAssistantRefereeOver = new EventType(24, "video_assistant_referee_over");

        //possible_video_assistant_referee
        public static readonly EventType PossibleVideoAssistantReferee = new EventType(25, "possible_video_assistant_referee");

        //offside
        public static readonly EventType Offside = new EventType(26, "offside");

        //penalty_awarded
        public static readonly EventType PenaltyAwarded = new EventType(27, "penalty_awarded");

        //period_score
        public static readonly EventType PeriodScore = new EventType(28, "period_score");

        // custom event 
        public static readonly EventType ScoreChangeByPenalty = new EventType(29, "score_change_by_penalty");
        public static readonly EventType ScoreChangeByOwnGoal = new EventType(30, "score_change_by_owngoal");

        public EventType()
        {
        }

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

        [IgnoreMember]
        public static readonly IDictionary<EventType, string> EventTypeNames = new Dictionary<EventType, string>
        {
            { EventType.BreakStart, "Break start" },
            { EventType.InjuryTimeShown, "Injury time shown" },
            { EventType.MatchEnded, "Match ended" },
            { EventType.MatchStarted, "Match started" },
            { EventType.PenaltyMissed, "Penalty Missed" },
            { EventType.PenaltyShootout, "Penalty shootout" },
            { EventType.PeriodStart, "Period start" },
            { EventType.RedCard, "Red card" },
            { EventType.ScoreChange, "Score change" },
            { EventType.Substitution, "Substitution" },
            { EventType.YellowCard, "Yellow card" },
            { EventType.YellowRedCard, "Yellow red card" },
            { EventType.CancelledVideoAssistantReferee, "Cancelled video assistant referee" },
            { EventType.CornerKick, "Corner kick" },
            { EventType.FreeKick, "Free kick" },
            { EventType.GoalKick, "Goal kick" },
            { EventType.Injury, "Injury" },
            { EventType.InjuryReturn, "Injury return" },
            { EventType.ShotOffTarget, "Shot off target" },
            { EventType.ShotOnTarget, "Shot on target" },
            { EventType.ShotSaved, "Shot saved" },
            { EventType.ThrowIn, "Throw in" },
            { EventType.VideoAssistantReferee, "Video assistant referee" },
            { EventType.VideoAssistantRefereeOver, "Video assistant referee over" },
            { EventType.PossibleVideoAssistantReferee, "Possible video assistant referee" },
            { EventType.Offside, "Off side" },
            { EventType.PenaltyAwarded, "Penalty awarded" },
            { EventType.PeriodScore, "Period score" }
        };
    };
}